using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score2x : MonoBehaviour
{


    public AudioClip liveGain;
    float score2xDuration = 10f;
    int score2xIndex = 0;
    Slider scoreSlider;
   

    private void Awake()
    {
        score2xDuration = GameManager.Instance.UpdatePowerUpsDuration(score2xIndex);
        scoreSlider = GameManager.Instance.score2xSlider.GetComponent<Slider>();
        scoreSlider.maxValue = score2xDuration;
        scoreSlider.value = score2xDuration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       

        if (collision.gameObject.CompareTag("Player"))
        {
   
            GameManager.Instance.audioSource.PlayOneShot(liveGain);
            GameManager.Instance.scoreIncrement = 2;   
            scoreSlider.gameObject.SetActive(true);
            scoreSlider.GetComponent<RectTransform>().sizeDelta = new Vector2((score2xDuration * 10f) + 100f, scoreSlider.GetComponent<RectTransform>().sizeDelta.y);
            scoreSlider.value = score2xDuration;
            gameObject.GetComponent<Image>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponentInChildren<TMP_Text>().enabled = false;

            Destroy(gameObject,score2xDuration);
        }
    }
    
    private void OnDestroy()
    {
        if (GameManager.Instance.activePlayerIndex != 1)
        {
            GameManager.Instance.scoreIncrement = 1;
        }
        scoreSlider.gameObject.SetActive(false);
     
    }

   
}
