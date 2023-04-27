using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagnetManager : MonoBehaviour
{


    public AudioClip MagnetGain;
   
    GameObject player;
    GameObject[] activeCoin;
    Slider magnetSlider;

    
    float magnetDuration = 10f;
    float speed = 2f;
    int magnetIndex = 2;

    private void Awake()
    {
        magnetDuration = GameManager.Instance.UpdatePowerUpsDuration(magnetIndex);
        magnetSlider = GameManager.Instance.MagnetSlider.GetComponent<Slider>();
        magnetSlider.maxValue = magnetDuration;
        magnetSlider.value = magnetDuration;
    }

    private void Update()
    {
        if(GameManager.Instance.magnetEnabled)
        {
            activeCoin = GameObject.FindGameObjectsWithTag("Coin");
            foreach (GameObject coin in activeCoin)
            {
                coin.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Vector2 direction = (GameManager.Instance.PlayerObj.GetComponent<RectTransform>().position-coin.GetComponent<RectTransform>().position).normalized;
                float distance = Vector2.Distance(GameManager.Instance.PlayerObj.GetComponent<RectTransform>().position, coin.GetComponent<RectTransform>().position);
                coin.GetComponent<RectTransform>().Translate(distance * speed * Time.deltaTime * direction);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
           
            player = collision.gameObject;
            GameManager.Instance.audioSource.PlayOneShot(MagnetGain);
            GameManager.Instance.magnetEnabled = true;
            magnetSlider.gameObject.SetActive(true);
            magnetSlider.GetComponent<RectTransform>().sizeDelta = new Vector2((magnetDuration * 10f) + 100f, magnetSlider.GetComponent<RectTransform>().sizeDelta.y);
            magnetSlider.value = magnetDuration;
            gameObject.GetComponent<Image>().enabled=false;
            gameObject.GetComponent<Collider2D>().enabled=false;
            

            Destroy(gameObject,magnetDuration);
            
        }
    }
    private void OnDestroy()
    {
        if(GameManager.Instance.activePlayerIndex!=3)
        {
            GameManager.Instance.magnetEnabled = false;
            
        }
        

        if (activeCoin!= null && activeCoin.Length > 0)
        {
            foreach (GameObject coin in activeCoin)
            {
                Destroy(coin);
            }
        }

        magnetSlider.gameObject.SetActive(false);
    }
}
