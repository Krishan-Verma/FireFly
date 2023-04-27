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
    public GameObject [] Enemies;
    public GameObject newlive;
    public GameObject magnet;
    public GameObject sheild;
    public GameObject score2x;
    public GameObject coin2x;
    public GameObject SpawnPos;
    public GameObject SpawnPosFront;
    public GameObject GamePanel;
    public GameObject PausePanel;
    public GameObject AdPanel;
    public GameObject EndPanel;
    public GameObject highScoreText;
    public GameObject Coins;
    public GameObject []Players;
    public GameObject score2xSlider;
    public GameObject coin2xSlider;
    public GameObject MagnetSlider;
    public GameObject PlayerObj;
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
    public AudioClip PlayerChange;

    public Button soundBtn;
    public Sprite soundSprite;
    public Sprite offSound;

    public PlayerManager player;

    int heartbeat = 6;
    int count = 0;
    float velocity;
    const int delay = 500;
    float lastInvisibleTime;
    float invisiblityTime;

    public int live;
    public int scoreCount = 0;
    public int coinCount = 0;
    int index = 0;

    public int scoreIncrement=1;
    public int coinIncrement = 1;
    public float speed = -200;
    public bool GodMode = false;
    public bool IsDead = false;
    private bool Spawning=true;
    public bool magnetEnabled = false;
    public int activePlayerIndex;
    GameObject gameObject1;

    Vector2 PowerUpPos;

    private void Awake()
    {
        PowerUpPos=new Vector2(-365f,345f);
        PlayerObj=Instantiate(Players[PlayerPrefs.GetInt("PlayerNo", 0)], SpawnPos.transform, false);
        speed *= (Screen.width / Screen.height);
        velocity = speed/10;
        scoreCount = 0;
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        invisiblityTime = 3f;
        AddPlayerAbility(PlayerPrefs.GetInt("PlayerNo", 0));

    }

    private void OnEnable()
    {

        IronSourceRewardedVideoEvents.onAdRewardedEvent += PlayerRevive;
        

    }

    public void AddPlayerAbility(int i)
    {   
        activePlayerIndex= i;
        
        if(gameObject1!=null)
        {   
            Destroy(gameObject1);
        }

        switch (i)
        {
            case 0: break;

            case 1: 
                    scoreIncrement = 2;
                    gameObject1 = Instantiate(score2x, SpawnPos.transform);
                    gameObject1.GetComponent<RectTransform>().localPosition= PowerUpPos;
                    break;

            case 2:
                    coinIncrement = 2;
                    gameObject1 = Instantiate(coin2x, SpawnPos.transform);
                    gameObject1.GetComponent<RectTransform>().localPosition = PowerUpPos;
                    break;

            case 3: magnetEnabled = true;
                    gameObject1 = Instantiate(magnet, SpawnPos.transform);
                    gameObject1.GetComponent<RectTransform>().localPosition = PowerUpPos;
                    break;
            
            case 4: GodMode = true;
                    gameObject1 = Instantiate(sheild, SpawnPos.transform);
                    gameObject1.GetComponent<Collider2D>().enabled = false;
                    gameObject1.transform.SetParent(PlayerObj.transform, false);
                    gameObject1.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
                    gameObject1.GetComponent<Rigidbody2D>().isKinematic = true;
                    break;


            default: break;

        }
    }



    private void OnDisable()
    {
        IronSourceRewardedVideoEvents.onAdRewardedEvent -= PlayerRevive;
        
    }

    void Start()
    {
        Music();

        StartCoroutine(Spawner(nameof(GenerateObstical), 1f,4f));
        StartCoroutine(Spawner(nameof(GenerateEnemies), 100f, 200f));
        StartCoroutine(Spawner(nameof(GenerateCoins), 1f, 5f));
        StartCoroutine(Spawner(nameof(GenerateExtras), 0f, 1f));
        StartCoroutine(Spawner(nameof(GenerateRandomPowerUps),10f,35f));
    }

    private void Update()
    {

        score.text = scoreCount.ToString();
        coinCountText.text = coinCount.ToString();

        if (scoreCount > 100000)
        { SceneManager.LoadScene(2); }
    }

    private void FixedUpdate()
    {
        if (!IsDead)
            scoreCount += scoreIncrement;


        if (scoreCount % delay == 0 && scoreCount / delay > count)
        {
            count++;
            speed += velocity;
  
        }

        if(score2xSlider.GetComponent<Slider>().value>0) {
            
            score2xSlider.GetComponent<Slider>().value -=Time.deltaTime;
        }

        if (coin2xSlider.GetComponent<Slider>().value > 0)
        {

            coin2xSlider.GetComponent<Slider>().value -= Time.deltaTime;
        }

        if (MagnetSlider.GetComponent<Slider>().value > 0)
        {

            MagnetSlider.GetComponent<Slider>().value -= Time.deltaTime;
        }
        


    }

    void GenerateObstical()
    {
        int randIndex = Random.Range(0, Obsticals.Length);
        float obsHeight = (randIndex < 9) ? Screen.height / 6f : Screen.height/1.5f;
        Instantiate(Obsticals[randIndex], new Vector3(Screen.width, obsHeight, 0f), Obsticals[randIndex].transform.rotation, SpawnPos.transform).GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0f);
       
    }
    void GenerateEnemies()
    {
        float obsHeight = Screen.height / 1.5f;
        Instantiate(Enemies[index], new Vector3(Screen.width, obsHeight, 0f), Enemies[index].transform.rotation, SpawnPos.transform).GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0f);
        index++;

        if (index % Enemies.Length == 0)
        {
            index = 0;
        }
    }



    void GenerateExtras()
    {
       int randIndex = Random.Range(0, extras.Length);
       Instantiate(extras[randIndex], new Vector3(Screen.width-50f, Screen.height / 8f, 0f), Quaternion.identity, SpawnPosFront.transform).GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0f);
     
    }
    void GenerateCoins()
    {
        Instantiate(Coins, new Vector3(Random.Range(Screen.width / 2, Screen.width), Random.Range(Screen.height / 4f, Screen.height / 1.5f), 0f), Quaternion.identity, SpawnPosFront.transform);

    }

    void GenerateNewLife()
    {
        Instantiate(newlive, new Vector3(Random.Range(Screen.width / 1.5f, Screen.width), Random.Range(Screen.height / 4f, Screen.height / 1.5f), 0f), Quaternion.identity, SpawnPos.transform).GetComponent<Rigidbody2D>().velocity = new Vector2(speed * 0.5f, 0f);

    } 
    void GenerateMagnet()
    {
        Instantiate(magnet, new Vector3(Random.Range(Screen.width / 1.5f, Screen.width), Random.Range(Screen.height / 4f, Screen.height / 1.5f), 0f), Quaternion.identity, SpawnPos.transform).GetComponent<Rigidbody2D>().velocity = new Vector2(speed * 0.5f, 0f);

    }
    void GenerateSheild()
    {
        Instantiate(sheild, new Vector3(Random.Range(Screen.width / 1.5f, Screen.width), Random.Range(Screen.height / 4f, Screen.height / 1.5f), 0f), Quaternion.identity, SpawnPos.transform).GetComponent<Rigidbody2D>().velocity = new Vector2(speed * 0.5f, 0f);

    }

   

    void GenerateScore2x()
    {
        Instantiate(score2x, new Vector3(Random.Range(Screen.width / 2, Screen.width), Random.Range(Screen.height / 4f, Screen.height / 1.5f), 0f), Quaternion.identity, SpawnPosFront.transform).GetComponent<Rigidbody2D>().velocity = new Vector2(speed * 0.5f, 0f);

    }

    void GenerateCoin2x()
    {
        Instantiate(coin2x, new Vector3(Random.Range(Screen.width / 2, Screen.width), Random.Range(Screen.height / 4f, Screen.height / 1.5f), 0f), Quaternion.identity, SpawnPosFront.transform).GetComponent<Rigidbody2D>().velocity = new Vector2(speed * 0.5f, 0f);

    }

    private void GenerateRandomPowerUps()
    {
        int randInt = Random.Range(0, 5); 

        switch(randInt)
        {
            case 0: GenerateScore2x(); break;
            case 1: GenerateCoin2x(); break;
            case 2: GenerateMagnet(); break;
            case 3: GenerateSheild(); break;
            case 4: GenerateNewLife(); break;


        }
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
        StopSpawning();
        EndPowerUpSliders();
        UpdateCoins();
        UpdateFinalScore();
        Invoke(nameof(HighScore), 4f);
        
        GamePanel.SetActive(false);
        EndPanel.SetActive(true);

        GameObject[] obsticals = GameObject.FindGameObjectsWithTag("Obstical");
        foreach(GameObject obstical in obsticals)
        Destroy(obstical);

        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coins");
        foreach (GameObject coin in coins)
        Destroy(coin);

        Destroy(GameObject.FindGameObjectWithTag("Live"));


    }

    public void ShowAdPanel(PlayerManager player)
    {
        Time.timeScale = 0f;
        AdPanel.SetActive(true);
        this.player = player;
        
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
        int i = (value>125)?value-125:0;
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
            audioMixer.SetFloat("MasterVolume", -60f);
            soundBtn.image.sprite = offSound;

        }
       else
        {
            soundBtn.image.sprite = soundSprite;
            audioMixer.SetFloat("MasterVolume", 10f);
        }

    }

    private void PlayerRevive(IronSourcePlacement arg1, IronSourceAdInfo arg2)
    {
        Invisible(player);
        player.transform.position = new Vector3(player.transform.position.x, Screen.height / 2f, 0f);
        Time.timeScale = 1;
       
    }


    private void RewardedVideoOnAdShowFailedEvent(IronSourceError arg1, IronSourceAdInfo arg2)
    {
        ObsticalManager.Instance.End(player);
    }

    private void RewardedVideoOnAdUnavailable()
    {
        ObsticalManager.Instance.End(player);

    }

    public void CallEnd()
    {
        ObsticalManager.Instance.End(player);
    }

    public bool Invisible(PlayerManager player)
    {

        if (lastInvisibleTime + invisiblityTime < Time.time)
        {
            lastInvisibleTime = Time.time;
            player.animator.SetTrigger("IsInvisible");
            return false;
        }
        else
        {

            return true;
        }
       
    }

    IEnumerator Spawner(string methodName, float startTime, float repeatTime)
    {
        yield return new WaitForSeconds(startTime);

        while (Spawning == true)
        { 
            SendMessage(methodName);
            repeatTime -= 0.002f;
            yield return new WaitForSeconds(repeatTime);
        }
    }

    void StopSpawning()
    {
        Spawning = false;
        StopAllCoroutines();
    }


    public float UpdatePowerUpsDuration(int powerUpIndex)
    {
      int starCount = PlayerPrefs.GetInt("Item" + powerUpIndex + "StarCount", 0);
      float duration;

      switch(starCount)
      {
            case 0: duration = 10f; break;
            case 1: duration = 15f; break;
            case 2: duration = 20f; break;
            case 3: duration = 25f; break;

            default: duration = 10f; break;
      }

     return duration;

    }

    void EndPowerUpSliders()
    {
        coin2xSlider.SetActive(false);
        score2xSlider.SetActive(false);
        MagnetSlider.SetActive(false);
        
    }

}

