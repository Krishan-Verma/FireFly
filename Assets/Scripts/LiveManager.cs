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
            if (Gamemanager.Instance.live <3)
            {
                Gamemanager.Instance.lives[Gamemanager.Instance.live].SetActive(true);
                Gamemanager.Instance.live += 1;
            }

            Gamemanager.Instance.audioSource.PlayOneShot(liveGain);
            Destroy(gameObject);
        }
    }
}
