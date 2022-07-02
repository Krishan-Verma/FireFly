using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Gamemanager : MonoBehaviour
{
    private float velocity = 100f;
    private const int delay = 300;
    public GameObject [] Obsticals;
    [System.NonSerialized]
    public float speed= -200;
    public GameObject SpawnPos;
    public GameObject EndPanel;
    public static Gamemanager Instance;
    public TMP_Text score;
    public TMP_Text Yourscore;
    public TMP_Text scoreboard;
    public TMP_Text scorepoint;
    public AudioSource audioSource;
    public AudioClip gameover;
    public GameObject Coins;
    public GameObject[] lives;
    public GameObject newlive;
    public TMP_Text godModeText;
    public int live=3;
    public bool GodMode = false;
    int heartbeat = 2;
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

    // Start is called before the first frame update
    void Start()
    {
        
       InvokeRepeating("GenerateObstical",1,5);
        Music();
        
    }

    void GenerateObstical()
    {
       Instantiate(Obsticals[0], new Vector3(Random.Range(Screen.width/1.5f, Screen.width), Random.Range(Screen.height / 5f,Screen.height/3f), 0f), Quaternion.identity,SpawnPos.transform).GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0f); 
       Instantiate(Obsticals[1], new Vector3(Random.Range(Screen.width/1.5f, Screen.width), Random.Range(Screen.height / 1.6f, Screen.height / 1.3f), 0f), Obsticals[1].transform.rotation, SpawnPos.transform).GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0f);
       Instantiate(Coins, new Vector3(Random.Range(Screen.width/2, Screen.width), Random.Range(Screen.height / 4f, Screen.height / 1.5f), 0f), Quaternion.identity, SpawnPos.transform);
    }

    public void Restart()
    {
        //audioSource.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
        EndPanel.SetActive(false);
        scoreCount = 0;
    }

    public void Menu()
    {
        Time.timeScale = 1;
        EndPanel.SetActive(false);
        SceneManager.LoadScene(0);
             
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        Yourscore.text = score.text;
        HighScore();
        scoreboard.enabled= false;
        scorepoint.enabled = false;
        EndPanel.SetActive(true);
        GameObject[] obsticals = GameObject.FindGameObjectsWithTag("Obstical");
        foreach(GameObject obstical in obsticals)
        Destroy(obstical);
    }

    private void Update()
    {
        
     score.text = (scoreCount).ToString();
    }

    private void FixedUpdate()
    {
        
        if(scoreCount% delay == 0 && scoreCount/ delay > count)
        {
            count++;
            speed += velocity;
            Instantiate(newlive, new Vector3(Random.Range(Screen.width / 1.5f, Screen.width), Random.Range(Screen.height / 4f, Screen.height / 1.5f), 0f), Quaternion.identity, SpawnPos.transform).GetComponent<Rigidbody2D>().velocity= new Vector2(speed*0.5f, 0f);
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
        if (int.Parse(Yourscore.text)>int.Parse(PlayerPrefs.GetString("HighScore", "0")))
        {
            PlayerPrefs.SetString("HighScore", Yourscore.text);
        }
    }

    public void EnterGodMode()
    {
        heartbeat--;
        if(heartbeat<0)
        {
            godModeText.gameObject.SetActive(true);
            Invoke("ChangeMode",1f);
            heartbeat = 2;
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
}

