using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class MenuManager : MonoBehaviour
{
    public AudioSource audioSource;
    public Button Sound;
    public Button BGMusic;
    public Button MusicBtn;
    public Button []PlayerOptions;
    public GameObject AdPanel;
    public GameObject RewardPanel;
    public GameObject FullPanel;
   
    public Sprite SoundSprite;
    public Sprite BGMusicSprite;
    public Sprite SelectedSprite;
    public Sprite UnSelectedSprite;
    public Sprite OffSound;

    public Sprite ZeroStarSprite;
    public Sprite OneStarSprite;
    public Sprite TwoStarSprite;
    public Sprite ThreeStarSprite;

    public Image[] PowerUPsStarImages;
   


    public static MenuManager Instance;
    public TMP_Text highScore;
    public TMP_Text TotalCoins;
    public TMP_Text AvilableCoinsInStore;
    public TMP_Text AvilableCoinsInPowerUps;
    private int CoinCheat;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        UpdateSprite();
        UpdateStarSprite();
        highScore.text = PlayerPrefs.GetString("HighScore", "000000");
        TotalCoins.text = PlayerPrefs.GetString("Coins", "000000");
        CoinCheat = PlayerPrefs.GetInt("Cheat", 0);
        AvilableCoinsInStore.text = TotalCoins.text;
        AvilableCoinsInPowerUps.text= TotalCoins.text;
        PlayerPrefs.SetString("Button" + 0, "Active");
        

    }

    private void OnEnable()
    {
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;

    }

    private void OnDisable()
    {
        IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;

    }

    private void Start()
    {
        for (int i = 0; i < PlayerOptions.Length; i++)
        {
            if (PlayerPrefs.GetString("Button" + i, "Lock") == "Active")
            {
                PlayerOptions[i].image.sprite = UnSelectedSprite;
            }
        }

        PlayerOptions[PlayerPrefs.GetInt("PlayerNo", 0)].image.sprite = SelectedSprite;
    }

    public void Play()
    {
        SceneManager.LoadScene(1);

    }

    public void Music()
    {
        if(audioSource.isPlaying)
        {
            MusicBtn.image.sprite = OffSound;
            audioSource.Pause();
        }
        else
        {
            MusicBtn.image.sprite = BGMusicSprite;
            audioSource.Play();
        }
    }

    public void PlaneSound()
    {
        if (PlayerPrefs.GetString("PlaneSound") == "ON")
        {
            Sound.image.sprite = OffSound;
            PlayerPrefs.SetString("PlaneSound", "OFF");
        }

        else
        {
            Sound.image.sprite = SoundSprite;
            PlayerPrefs.SetString("PlaneSound", "ON");

        }
    }

    public void BackGroundMusic()
    {
        if (PlayerPrefs.GetString("BackGroundMusic") == "ON")
        {
            BGMusic.image.sprite = OffSound;
            PlayerPrefs.SetString("BackGroundMusic", "OFF");
        }

        else
        {
            BGMusic.image.sprite = BGMusicSprite;
            PlayerPrefs.SetString("BackGroundMusic", "ON");

        }

    }

    public void UpdateSprite()
    {

        if (PlayerPrefs.GetString("PlaneSound","ON") == "ON")
        {
            Sound.image.sprite = SoundSprite;
            PlayerPrefs.SetString("PlaneSound", "ON");
            
        }

        else
        {
            Sound.image.sprite = OffSound;
            

        }

        if (PlayerPrefs.GetString("BackGroundMusic","ON") == "ON")
        {

            BGMusic.image.sprite = BGMusicSprite;
            PlayerPrefs.SetString("BackGroundMusic", "ON");
        }

        else
        {
            
            BGMusic.image.sprite = OffSound;

        }
    }

    
    public void Exit()
    {
        Application.Quit();

    }

    public void SelectPlayer(int i)
    {
        if (PlayerPrefs.GetString("Button" + i,"Lock")=="Active")
        {
            MakeSelection(i);
        }
        else
        {
            if (MakePayment(i))
            {
                MakeSelection(i);

                PlayerPrefs.SetString("Button" + i, "Active");
            }
        }
    }

    private void MakeSelection(int i)
    {
        foreach (Button Player in PlayerOptions)
        {
            if (Player.image.sprite.name == SelectedSprite.name)
            {
                Player.image.sprite = UnSelectedSprite;

            }
        }

        PlayerOptions[i].image.sprite = SelectedSprite;
        PlayerPrefs.SetInt("PlayerNo", i);
    }


    private bool MakePayment(int playerIndex)
    {
        int price=0;
        int coins = int.Parse(PlayerPrefs.GetString("Coins", "0"));

        switch (playerIndex)
        {
            case 0: price = 0; break;

            case 1: price = 100000; break;

            case 2: price = 300000; break;

            case 3: price = 500000; break;

            case 4: price = 1000000;
                if (CoinCheat == 7)
                {
                    CoinCheat++;
                    PlayerPrefs.SetString("Coins", "1000000");
                    PlayerPrefs.SetInt("Cheat", CoinCheat);
                }
                else
                    CoinCheat++;
                    break;

            default: price = 0; break;

        }

        if(coins>=price)
        {
            coins -= price;
            PlayerPrefs.SetString("Coins", coins.ToString());
            TotalCoins.text = coins.ToString();
            AvilableCoinsInStore.text= coins.ToString();
            AvilableCoinsInPowerUps.text= coins.ToString();
            return true; 

        }
        else
        {
            AdPanel.SetActive(true);
            return false;
        }
               
    }

    public void SelectPowerUp(int itemIndex)
    {
        if(PlayerPrefs.GetString("PowerUp" + itemIndex, "Boostable")!="Full")
        {
            if (BoostPowerUP(itemIndex))
            {
                UpdateStarCount(itemIndex);

            }
            else
            {
                AdPanel.SetActive(true);
            }

        }
        else
        {
            FullPanel.SetActive(true); 
        }
    }

    private void UpdateStarSprite()
    {
       

        for (int i = 0; i < PowerUPsStarImages.Length; i++)
        {
            int starCount = PlayerPrefs.GetInt("Item" + i + "StarCount", 0);

            switch (starCount)
            {
                case 0:
                    PowerUPsStarImages[i].sprite = ZeroStarSprite; break;

                case 1:
                    PowerUPsStarImages[i].sprite = OneStarSprite; break;

                case 2:
                    PowerUPsStarImages[i].sprite = TwoStarSprite; break;

                case 3:
                    PowerUPsStarImages[i].sprite = ThreeStarSprite; break;

                default: break;


            }
        }
        
    }

    private void UpdateStarCount(int itemIndex)
    {
       int starCount = PlayerPrefs.GetInt("Item" + itemIndex + "StarCount", 0);
        
       if(starCount<3)
       {
            starCount++;
            PlayerPrefs.SetInt("Item" + itemIndex + "StarCount", starCount);
            UpdateStarSprite();

            if(starCount==3)
            {
                PlayerPrefs.SetString("PowerUp" + itemIndex,"Full");
            }
        }
       

   
    }

    private bool BoostPowerUP(int powerUpIndex)
    {
        int price;
        int coins = int.Parse(PlayerPrefs.GetString("Coins", "0"));

        switch (powerUpIndex)
        {
   
            case 0: price = 1000; break;

            case 1: price = 3000; break;

            case 2: price = 5000; break;

            case 3: price = 10000;break;

            default: price = 0; break;

        }

        if (coins >= price)
        {
            coins -= price;
            PlayerPrefs.SetString("Coins", coins.ToString());
            TotalCoins.text = coins.ToString();
            AvilableCoinsInStore.text = coins.ToString();
            AvilableCoinsInPowerUps.text = coins.ToString();

            return true;

        }
        else
        {
            return false;
        }

    }

    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {

        int rewardCoins = 1000;
        int coinsTotal = int.Parse(PlayerPrefs.GetString("Coins", "0")) + rewardCoins;

        PlayerPrefs.SetString("Coins", string.Format("{0:D6}", coinsTotal));
        TotalCoins.text = coinsTotal.ToString();
        AvilableCoinsInStore.text = coinsTotal.ToString();
        AvilableCoinsInPowerUps.text=coinsTotal.ToString();
        RewardPanel.SetActive(true);
    }

}
