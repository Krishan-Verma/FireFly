using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Coin2x : MonoBehaviour
{


    public AudioClip liveGain;
    float coin2xDuration = 10f;
    int coin2xIndex = 1;
    Slider coinSlider;

    private void Awake()
    {
        coin2xDuration = GameManager.Instance.UpdatePowerUpsDuration(coin2xIndex);
        coinSlider = GameManager.Instance.coin2xSlider.GetComponent<Slider>();
        coinSlider.maxValue = coin2xDuration;
        coinSlider.value = coin2xDuration;

       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
   
            GameManager.Instance.audioSource.PlayOneShot(liveGain);
            GameManager.Instance.coinIncrement = 2;
            coinSlider.gameObject.SetActive(true);
            coinSlider.GetComponent<RectTransform>().sizeDelta = new Vector2((coin2xDuration * 10f) + 100f, coinSlider.GetComponent<RectTransform>().sizeDelta.y);
            coinSlider.value = coin2xDuration;
            gameObject.GetComponent<Image>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponentInChildren<TMP_Text>().enabled = false;

            Destroy(gameObject,coin2xDuration);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.coinIncrement = 1;
        coinSlider.gameObject.SetActive(false);
    }
}
