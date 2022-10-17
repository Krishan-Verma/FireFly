using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveManager : MonoBehaviour
{


    public AudioClip liveGain;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            if (GameManager.Instance.live <3)
            {
                GameManager.Instance.lives[GameManager.Instance.live].SetActive(true);
                GameManager.Instance.live += 1;
            }

            GameManager.Instance.audioSource.PlayOneShot(liveGain);
            Destroy(gameObject);
        }
    }
}
