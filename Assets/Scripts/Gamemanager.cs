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
    public GameObject SpawnPos;
    public GameObject SpawnPosFront;
    public GameObject GamePanel;
    public GameObject PausePanel;
    public GameObject AdPanel;
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


    public float speed = -200;
    public float ObsticalSpawnTime=4f;
    public bool GodMode = false;
    public bool IsDead = false;
    private bool Spawning=true;

    private void Awake()
    {
        
        Instantiate(Players[PlayerPrefs.GetInt("PlayerNo", 0)], SpawnPos.transform, false);
        speed *= (Screen.width / Screen.height);
        velocity = speed/10;
        scoreCount = 0;
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        invisiblityTime = 3f;


    }

    private void OnEnable()
    {
        IronSourceRewardedVideoEvents.onAdRewardedEvent += PlayerRevive;
        

    }

  

    private void OnDisable()
    {
        IronSourceRewardedVideoEvents.onAdRewardedEvent -= PlayerRevive;
        
    }

    void Start()
    {
        Music();

        StartCoroutine(Spawner(nameof(GenerateObstical), 1f, ObsticalSpawnTime));
        StartCoroutine(Spawner(nameof(GenerateEnemies), 100f, 200f));
        StartCoroutine(Spawner(nameof(GenerateCoins), 1f, 3f));
        StartCoroutine(Spawner(nameof(GenerateExtras), 0f, 1f));
        StartCoroutine(Spawner(nameof(GenerateNewLife), 50f, 50f)); 


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
            ObsticalSpawnTime -= 0.001f;
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

    void GenerateNewLife()
    {
        Instantiate(newlive, new Vector3(Random.Range(Screen.width / 1.5f, Screen.width), Random.Range(Screen.height / 4f, Screen.height / 1.5f), 0f), Quaternion.identity, SpawnPos.transform).GetComponent<Rigidbody2D>().velocity = new Vector2(speed * 0.5f, 0f);

    }

    void GenerateCoins()
    {
        Instantiate(Coins, new Vector3(Random.Range(Screen.width / 2, Screen.width), Random.Range(Screen.height / 4f, Screen.height / 1.5f), 0f), Quaternion.identity, SpawnPosFront.transform);

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
            audioMixer.SetFloat("MasterVolume", -80f);
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
}

