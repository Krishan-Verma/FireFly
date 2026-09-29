using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.LevelPlay;

public class ISScript : MonoBehaviour
{
    const string appKey = "1942a5b85";
    // Rewarded ad unit ID for this app, from the LevelPlay dashboard's Ad Units page.
    const string rewardedAdUnitId = "o1mvnyjcotsveeqs";

    public GameObject AdPanel;
    public GameObject NoAdPanel;
    public static ISScript Instance;

    public static event Action OnAdRewarded;

    // ISScript is placed in several scenes, but the SDK and the ad are set up once per session.
    static bool isInitStarted;
    static LevelPlayRewardedAd rewardedAd;
    static bool isRewardEarned;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (isInitStarted)
        {
            if (rewardedAd != null && !rewardedAd.IsAdReady())
                rewardedAd.LoadAd();
            return;
        }

        isInitStarted = true;
        LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed += SdkInitializationFailedEvent;
        LevelPlay.Init(appKey);
    }

    static void SdkInitializationCompletedEvent(LevelPlayConfiguration config)
    {
        LevelPlay.ValidateIntegration();

        rewardedAd = new LevelPlayRewardedAd(rewardedAdUnitId);
        rewardedAd.OnAdRewarded += (adInfo, reward) => GrantReward();
        rewardedAd.OnAdClosed += adInfo =>
        {
            if (Instance != null)
                Instance.StartCoroutine(Instance.OfferAdAgainIfNotRewarded());
            rewardedAd.LoadAd();
        };
        rewardedAd.OnAdDisplayFailed += (adInfo, error) =>
        {
            Debug.LogWarning("Rewarded ad failed to show: " + error);
            if (Instance != null)
                Instance.ShowNoAdPanel();
            rewardedAd.LoadAd();
        };
        rewardedAd.OnAdLoadFailed += error => Debug.LogWarning("Rewarded ad failed to load: " + error);
        rewardedAd.LoadAd();
    }

    static void SdkInitializationFailedEvent(LevelPlayInitError error)
    {
        Debug.LogWarning("LevelPlay init failed: " + error);

        // Try again when the next scene loads.
        LevelPlay.OnInitSuccess -= SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed -= SdkInitializationFailedEvent;
        isInitStarted = false;
    }

    //Rewarded
    public void ShowRewardedAd()
    {
        isRewardEarned = false;
#if UNITY_EDITOR
        // LevelPlay's own Editor test ad can't be closed when Active Input Handling is
        // "Input System Package", and its timer stops while the game is paused.
        StartCoroutine(ShowEditorTestAd());
#else
        if (rewardedAd != null && rewardedAd.IsAdReady())
        {
            rewardedAd.ShowAd();
        }
        else
        {
            ShowNoAdPanel();
            rewardedAd?.LoadAd();
        }
#endif
    }

    static void GrantReward()
    {
        Debug.Log("Rewarded ad: reward earned");
        isRewardEarned = true;
        OnAdRewarded?.Invoke();
    }

    // The reward callback can arrive just after the ad closes, so wait before treating the ad as
    // skipped. Realtime, because the game is paused (timeScale 0) while the ad panel is up.
    IEnumerator OfferAdAgainIfNotRewarded()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (!isRewardEarned)
        {
            Debug.Log("Rewarded ad closed before the reward was earned");
            AdPanel.SetActive(true);
        }
    }

    void ShowNoAdPanel()
    {
        AdPanel.SetActive(false);
        NoAdPanel.SetActive(true);
    }

#if UNITY_EDITOR
    // Editor stand-in for a rewarded ad. Like a real one, it only rewards the player for watching
    // until the timer ends; skipping gives no reward.
    IEnumerator ShowEditorTestAd()
    {
        const float adSeconds = 5f;

        var adObject = new GameObject("Editor Test Ad", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = adObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;
        var scaler = adObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);

        var background = CreateUIObject<Image>("Background", adObject.transform);
        background.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
        Stretch(background.rectTransform);

        var message = CreateUIObject<TextMeshProUGUI>("Message", adObject.transform);
        message.fontSize = 60f;
        message.alignment = TextAlignmentOptions.Center;
        Stretch(message.rectTransform);

        var skipImage = CreateUIObject<Image>("Skip", adObject.transform);
        skipImage.color = new Color(1f, 1f, 1f, 0.25f);
        var skipRect = skipImage.rectTransform;
        skipRect.anchorMin = skipRect.anchorMax = skipRect.pivot = Vector2.one;
        skipRect.anchoredPosition = new Vector2(-40f, -40f);
        skipRect.sizeDelta = new Vector2(400f, 110f);
        var skipLabel = CreateUIObject<TextMeshProUGUI>("Label", skipRect);
        skipLabel.text = "Skip (no reward)";
        skipLabel.fontSize = 40f;
        skipLabel.alignment = TextAlignmentOptions.Center;
        Stretch(skipLabel.rectTransform);

        bool skipped = false;
        var skipButton = skipImage.gameObject.AddComponent<Button>();
        skipButton.targetGraphic = skipImage;
        skipButton.onClick.AddListener(() => skipped = true);

        float endTime = Time.realtimeSinceStartup + adSeconds;
        while (!skipped && Time.realtimeSinceStartup < endTime)
        {
            message.text = "Editor test ad\nReward in " + Mathf.CeilToInt(endTime - Time.realtimeSinceStartup) + " s";
            yield return null;
        }

        Destroy(adObject);
        if (!skipped)
            GrantReward();
        yield return OfferAdAgainIfNotRewarded();
    }

    static T CreateUIObject<T>(string name, Transform parent) where T : Component
    {
        var uiObject = new GameObject(name, typeof(RectTransform), typeof(T));
        uiObject.transform.SetParent(parent, false);
        return uiObject.GetComponent<T>();
    }

    static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = rect.offsetMax = Vector2.zero;
    }
#endif
}
