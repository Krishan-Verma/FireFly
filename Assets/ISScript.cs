using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ISScript : MonoBehaviour
{
    // Start is called before the first frame update

    string appkey= "1942a5b85";
    public GameObject RewardPanel;
    public GameObject NoAdPanel;

    void Start()
    {
        //For Rewarded Video
        IronSource.Agent.init(appkey, IronSourceAdUnits.REWARDED_VIDEO);
        //For Interstitial
    //    IronSource.Agent.init(appkey, IronSourceAdUnits.INTERSTITIAL);
    //    For Offerwall
    //    IronSource.Agent.init(appkey, IronSourceAdUnits.OFFERWALL);
    //    For Banners
    //    IronSource.Agent.init(appkey, IronSourceAdUnits.BANNER);
    
    }

    private void OnEnable()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;

        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;

    }
    // Update is called once per frame
    void Update()
    {
        
    }

   
    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }


    private void SdkInitializationCompletedEvent() 
    {
        IronSource.Agent.validateIntegration();
    }

    //Rewarded
    public void ShowRewardedAd()
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            RewardPanel.SetActive(true);
            IronSource.Agent.showRewardedVideo();
            
        }
        else
        {
            NoAdPanel.SetActive(true);
            Debug.Log("Ad is not ready");
            
        }
    }


    /************* RewardedVideo AdInfo Delegates *************/
    // Indicates that there’s an available ad.
    // The adInfo object includes information about the ad that was loaded successfully
    // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
    }
    // Indicates that no ads are available to be displayed
    // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    void RewardedVideoOnAdUnavailable()
    {
    }
    // The Rewarded Video ad view has opened. Your activity will loose focus.
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
    }
    // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        
    }
    // The user completed to watch the video, and should be rewarded.
    // The placement parameter will include the reward data.
    // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
       

        int rewardCoins = 1000;
        int coinsTotal = int.Parse(PlayerPrefs.GetString("Coins", "0")) + rewardCoins;

        PlayerPrefs.SetString("Coins", string.Format("{0:D6}", coinsTotal));
        MenuManager.Instance.TotalCoins.text = coinsTotal.ToString();
        MenuManager.Instance.AvilabeCoins.text = coinsTotal.ToString();
        

    }
    // The rewarded video ad was failed to show.
    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
    }
    // Invoked when the video ad was clicked.
    // This callback is not supported by all networks, and we recommend using it only if
    // it’s supported by all networks you included in your build.
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
    }


}
