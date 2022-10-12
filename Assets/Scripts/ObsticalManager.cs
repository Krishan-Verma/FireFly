using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticalManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hitsound;
    float lastInvisibleTime;
    float invisiblityTime;
    

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        invisiblityTime = 2f;

    }

    void DisablePlayer(PlayerManager player)
    {
        
        player.tag = "Dead";
        player.animator.SetBool("IsDead", true);
        player.audioSource.Stop();
        player.enabled = false;
        Gamemanager.Instance.audioSource.Stop();
        Gamemanager.Instance.audioSource.PlayOneShot(Gamemanager.Instance.gameOverClip);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.tag=="Player" && !Invisible(collision.gameObject.GetComponent<PlayerManager>()))
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

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player" && !Invisible(collision.gameObject.GetComponent<PlayerManager>()))
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
        if(collision.gameObject.tag=="Player" &&!Invisible(collision.gameObject.GetComponent<PlayerManager>()))
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

   void DecreaseLive(RectTransform playerPosition)
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
            Gamemanager.Instance.spawnTime = (Gamemanager.Instance.spawnTime > 1) ? Gamemanager.Instance.spawnTime - 1f : 1f;
        }
        
        playerPosition.position = new Vector3(playerPosition.position.x, Screen.height / 2f,0f);
                
    }

   bool Invisible(PlayerManager player)
   {
        if(lastInvisibleTime+invisiblityTime<Time.time)
        {
            lastInvisibleTime = Time.time;
            player.animator.SetTrigger("IsInvisible");
            return false;
        }
        else
        {
            
            return true;
        }
        
   }
}
