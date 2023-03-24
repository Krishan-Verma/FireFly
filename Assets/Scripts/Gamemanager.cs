using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject []Obsticals;
    public GameObject []extras;
    public GameObject []lives;
    public GameObject newlive;
    public GameObject SpawnPos;
    public GameObject GamePanel;
    public GameObject PausePanel;
    public GameObject EndPanel;
    public GameObject highScoreText;
    public GameObject Coins;
    public GameObject []Players;
    

    public TMP_Text score;
    public TMP_Text finalScore;
    public TMP_Text coinCountText;
    public TMP_Text finalCoinCount;
    public TMP_Text godModeText;


    public AudioSource audioSource;
    public AudioMixer audioMixer;
    public AudioClip gameOverClip;
    public AudioClip scoreClip;
    public AudioClip highScoreClip;

    public Button soundBtn;
    public Sprite soundSprite;
    public Sprite offSound;

    int heartbeat = 6;
    int count = 0;
    float velocity;
    const int delay = 500;


    public int live;
    public int scoreCount = 0;
    public int coinCount = 0;


    public float speed = -200;
    public float ObsticalSpawnTime=5f;
    public bool GodMode = false;
    public bool IsDead = false;
    

    private void Awake()
    {
        Instantiate(Players[PlayerPrefs.GetInt("PlayerNo", 0)], SpawnPos.transform, false);
        speed *= (Screen.width / Screen.height);
        velocity = speed/3;
        scoreCount = 0;
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        
       
    }


    void Start()
    {
        
       InvokeRepeating(nameof(GenerateObstical), 1f, ObsticalSpawnTime);
       InvokeRepeating(nameof(GenerateExtras), 0f, 2f);
       InvokeRepeating(nameof(GenerateNewLife), 50f, 50f);
       Music();
        
    }

    private void Update()
    {

        score.text = scoreCount.ToString();
        coinCountText.text = coinCount.ToString();

    }

    private void FixedUpdate()
    {
        if (!IsDead)
            scoreCount += 1;


        if (scoreCount % delay == 0 && scoreCount / delay > count)
        {
            count++;
            speed += velocity;
        }
    }

    void GenerateObstical()
    {
        int randIndex = Random.Range(0, Obsticals.Length);
        float obsHeight = (randIndex < 3) ? Screen.height / 6f : Screen.height/1.5f;
        Instantiate(Coins, new Vector3(Random.Range(Screen.width / 2, Screen.width), Random.Range(Screen.height / 4f, Screen.height / 1.5f), 0f), Quaternion.identity, SpawnPos.transform);
        Instantiate(Obsticals[randIndex], new Vector3(Screen.width, obsHeight, 0f), Quaternion.identity, SpawnPos.transform).GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0f);
       
    }

    void GenerateExtras()
    {
       int randIndex = Random.Range(0, extras.Length);
       Instantiate(extras[randIndex], new Vector3(Screen.width-50f, Screen.height / 8f, 0f), Quaternion.identity, SpawnPos.transform).GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0f);
     
    }

    void GenerateNewLife()
    {
        Instantiate(newlive, new Vector3(Random.Range(Screen.width / 1.5f, Screen.width), Random.Range(Screen.height / 4f, Screen.height / 1.5f), 0f), Quaternion.identity, SpawnPos.transform).GetComponent<Rigidbody2D>().velocity = new Vector2(speed * 0.5f, 0f);

    }

    public void Restart()
    {
        Time.timeScale = 1f;
        EndPanel.SetActive(false);
        GamePanel.SetActive(true);
        scoreCount = 0;
        audioMixer.SetFloat("MasterVolume", 10f);
        SceneManager.LoadScene(1);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        EndPanel.SetActive(false);
        GamePanel.SetActive(true);
        audioMixer.SetFloat("MasterVolume", 10f);
        SceneManager.LoadScene(0);
             
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        PausePanel.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        PausePanel.SetActive(false);
    }

    public void EndGame()
    {
       
        CancelInvoke();
        UpdateCoins();
        UpdateFinalScore();
        Invoke("HighScore",4f);
        
        GamePanel.SetActive(false);
        EndPanel.SetActive(true);

        GameObject[] obsticals = GameObject.FindGameObjectsWithTag("Obstical");
        foreach(GameObject obstical in obsticals)
        Destroy(obstical);

        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coins");
        foreach (GameObject coin in coins)
        Destroy(coin);
    }

   

    public void Music()
    {

        if (PlayerPrefs.GetString("BackGroundMusic") == "ON")
        {
            audioSource.Play();
        }
        else 
        {
            audioSource.Stop();
        }
    }

    public void HighScore()
    {
        if (int.Parse(finalScore.text)>int.Parse(PlayerPrefs.GetString("HighScore", "0")))
        {
            PlayerPrefs.SetString("HighScore", string.Format("{0:D6}",scoreCount));
            audioSource.PlayOneShot(highScoreClip);
            highScoreText.SetActive(true);
        }
    }

    public void UpdateCoins()
    {
        int coinsTotal = int.Parse(PlayerPrefs.GetString("Coins", "0"))+coinCount;
        
        PlayerPrefs.SetString("Coins",string.Format("{0:D6}", coinsTotal));
      
    }
    
    public void EnterGodMode()
    {
        heartbeat--;
        if(heartbeat<0)
        {
            godModeText.gameObject.SetActive(true);
            Invoke("ChangeMode",1f);
            heartbeat = 6;
        }
    }

    private void ChangeMode()
    {

        if (GodMode)
            godModeText.text = "God Mode Activated";
        else
            godModeText.text = "God Mode Deactivated";

        GodMode = !GodMode;


        godModeText.gameObject.SetActive(false);
    }

    void UpdateFinalScore()
    {
        
        audioSource.PlayOneShot(scoreClip);
        StartCoroutine(UpdateScore(coinCount, finalCoinCount));
        StartCoroutine(UpdateScore(scoreCount, finalScore));
       
             
    }

    IEnumerator UpdateScore(int value,TMP_Text text)
    {
        int i = (value>200)?value-200:0;
        while(i<=value)
        {
            text.text= i.ToString();
            yield return null;
            i++;
            
        }
        
    }

    
    public void Sounds()
    {
       audioMixer.GetFloat("MasterVolume", out float vol);
       if(vol==10f)
        {
            audioMixer.SetFloat("MasterVolume", -80f);
            soundBtn.image.sprite = offSound;

        }
       else
        {
            soundBtn.image.sprite = soundSprite;
            audioMixer.SetFloat("MasterVolume", 10f);
        }

    }
}

