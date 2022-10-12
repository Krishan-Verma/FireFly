using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField]
    AudioClip coinSound;
    [SerializeField]
    float coinSpeed;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<Rigidbody2D>().velocity = new Vector2(Gamemanager.Instance.speed, 0f); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            audioSource.PlayOneShot(coinSound);
            Gamemanager.Instance.coinCount ++;
            Destroy(gameObject,0.4f);
        }
    }
}
