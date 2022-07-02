using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticalManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hitsound;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void DisablePlayer(PlayerManager player)
    {
        
        player.tag = "Dead";
        player.animator.SetBool("IsDead", true);
        player.audioSource.Stop();
        player.enabled = false;
        Gamemanager.Instance.audioSource.Stop();
        Gamemanager.Instance.audioSource.PlayOneShot(Gamemanager.Instance.gameover);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.tag=="Player")
        {
            if (Gamemanager.Instance.live > 0)
            {
                DecreaseLive(collision.gameObject.GetComponent<RectTransform>());
            }
            else
            {
                DisablePlayer(collision.gameObject.GetComponent<PlayerManager>());
                StartCoroutine(LoadEndGame(3f));
            }
           
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {

            if (Gamemanager.Instance.live >0)
            {
                DecreaseLive(collision.gameObject.GetComponent<RectTransform>());
            }
            else
            {
                DisablePlayer(collision.gameObject.GetComponent<PlayerManager>());
                StartCoroutine(LoadEndGame(3f));
            }
        }
    }

    
    IEnumerator LoadEndGame(float second)
    {
        yield return new WaitForSeconds(second);
        Gamemanager.Instance.EndGame();
    }

    public void DecreaseLive(RectTransform playerPosition)
    {
        if (!Gamemanager.Instance.GodMode)
        {
            Gamemanager.Instance.live -= 1;
            audioSource.PlayOneShot(hitsound);
            Gamemanager.Instance.lives[Gamemanager.Instance.live].SetActive(false);
        }
        else
        {
            Gamemanager.Instance.speed -= 100f;
        }
        
        playerPosition.position = new Vector3(playerPosition.position.x, Screen.height / 2f,0f);
        
    }
}
