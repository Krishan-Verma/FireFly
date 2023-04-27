using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    AudioSource audioSource;
    public int PlayerNo;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.PlayerChange);
            Destroy(collision.gameObject);
            GameManager.Instance.PlayerObj=Instantiate(GameManager.Instance.Players[PlayerNo],GameManager.Instance.SpawnPos.transform, false); ;
            GameManager.Instance.AddPlayerAbility(PlayerNo);
            Destroy(gameObject);
        }
    }
}
