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
    public Sprite SoundSprite;
    public Sprite BGMusicSprite;
    public Sprite OffSound;
    public static MenuManager Instance;
    public TMP_Text highScore;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        UpdateSprite();
        highScore.text = PlayerPrefs.GetString("HighScore", "0");

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
}
