using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class MenuManager : MonoBehaviour
{
    public AudioSource audioSource;
    public Button Sound;
    public Button BGMusic;
    public Button MusicBtn;
    public Button []PlayerOptions;
    public Sprite SoundSprite;
    public Sprite BGMusicSprite;
    public Sprite SelectedSprite;
    public Sprite UnSelectedSprite;
    public Sprite OffSound;

    public static MenuManager Instance;
    public TMP_Text highScore;
    public TMP_Text TotalCoins;
    public TMP_Text AvilabeCoins;
    private int CoinCheat = 0;

    private void Awake()
    {
        
  
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        UpdateSprite();
        highScore.text = PlayerPrefs.GetString("HighScore", "000000");
        TotalCoins.text = PlayerPrefs.GetString("Coins", "000000");
        AvilabeCoins.text = TotalCoins.text;
        PlayerPrefs.SetString("Button" + 0, "Active");
        

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
                         PlayerPrefs.SetString("Coins", "1000000");
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
            AvilabeCoins.text= coins.ToString();

            return true; 

        }
        else
        {
            Debug.Log("Need More Coins!");
            return false;
        }
               
    }
}
