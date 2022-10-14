using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Gamemanager : MonoBehaviour
{
    private float velocity = 100f;
    private const int delay = 1000;
    public GameObject [] Obsticals;
    public GameObject[] extras;
    [System.NonSerialized]
    public float speed= -200;
    public GameObject SpawnPos;
    public GameObject GamePanel;
    public GameObject EndPanel;
    public static Gamemanager Instance;
    public TMP_Text score;
    public TMP_Text finalScore;
    public TMP_Text coinCountText;
    public TMP_Text finalCoinCountText;
    public AudioSource audioSource;
    public AudioClip gameOverClip;
    public AudioClip scoreClip;
   
    public AudioClip highScoreClip;
    public GameObject highScoreText;
    public GameObject Coins;
    public GameObject[] lives;
    public GameObject newlive;
    public TMP_Text godModeText;
    public int live;
    public bool GodMode = false;
    public float spawnTime=5f;
    public int coinCount = 0;
    int heartbeat = 6;
    int count = 0;
   
    

    public int scoreCount = 0;
    private void Awake()
    {
      
        speed *= (Screen.width / Screen.height);
        velocity = speed;
        scoreCount = 0;
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

   
    void Start()
    {
        
       InvokeRepeating(nameof(GenerateObstical), 1f,spawnTime);
       InvokeRepeating(nameof(GenerateExtras), 0f, 2f);
       InvokeRepeating(nameof(GenerateNewLife), 50f, 50f);
        Music();
        
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
       Instantiate(extras[randIndex], new Vector3(Screen.width-50f, Screen.height / 6f, 0f), Quaternion.identity, SpawnPos.transform).GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0f);
     
    }

    void GenerateNewLife()
    {
        Instantiate(newlive, new Vector3(Random.Range(Screen.width / 1.5f, Screen.width), Random.Range(Screen.height / 4f, Screen.height / 1.5f), 0f), Quaternion.identity, SpawnPos.transform).GetComponent<Rigidbody2D>().velocity = new Vector2(speed * 0.5f, 0f);

    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
        EndPanel.SetActive(false);
        GamePanel.SetActive(true);
        scoreCount = 0;
    }

    public void Menu()
    {
      
        EndPanel.SetActive(false);
        GamePanel.SetActive(true);
        SceneManager.LoadScene(0);
             
    }

    public void EndGame()
    {
       
        CancelInvoke();
        finalScore.text = score.text;
        
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

    private void Update()
    {
        
     score.text = scoreCount.ToString();
     coinCountText.text = coinCount.ToString();

    }

    private void FixedUpdate()
    {
        scoreCount += 1;
      

        if(scoreCount% delay == 0 && scoreCount/ delay > count)
        {
            count++;
            speed += velocity;
        }
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
            PlayerPrefs.SetString("HighScore", finalScore.text);
            audioSource.PlayOneShot(highScoreClip);
            highScoreText.SetActive(true);
        }
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
        StartCoroutine(UpdateScore(coinCount, finalCoinCountText));
        StartCoroutine(UpdateScore(scoreCount, finalScore));
       
             
    }

    IEnumerator UpdateScore(int value,TMP_Text text)
    {
        int i = (value>200)?value-200:0;
        while(i<value)
        {
            text.text= i.ToString();
            yield return null;
            i++;
            
        }
        
    }
}

