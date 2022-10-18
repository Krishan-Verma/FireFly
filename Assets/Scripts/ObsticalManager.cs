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
        player.GetComponent<Rigidbody2D>().gravityScale = 10f;
        player.enabled = false;
       
        GameManager.Instance.audioSource.Stop();
        GameManager.Instance.audioSource.PlayOneShot(GameManager.Instance.gameOverClip);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.tag=="Player" && !Invisible(collision.gameObject.GetComponent<PlayerManager>()))
        {
            
            if (GameManager.Instance.live > 0)
            {
                
                DecreaseLive(collision.gameObject.GetComponent<RectTransform>());


            }
            else
            {
                DisablePlayer(collision.gameObject.GetComponent<PlayerManager>());
                StartCoroutine(LoadEndGame(3f, collision.gameObject.GetComponent<PlayerManager>()));
            }
           
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player" && !Invisible(collision.gameObject.GetComponent<PlayerManager>()))
        {

            if (GameManager.Instance.live > 0)
            {

                DecreaseLive(collision.gameObject.GetComponent<RectTransform>());


            }
            else
            {
                DisablePlayer(collision.gameObject.GetComponent<PlayerManager>());
                StartCoroutine(LoadEndGame(3f, collision.gameObject.GetComponent<PlayerManager>()));
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if(collision.gameObject.tag=="Player" &&!Invisible(collision.gameObject.GetComponent<PlayerManager>()))
        {
           
            if (GameManager.Instance.live >0)
            {
                
                DecreaseLive(collision.gameObject.GetComponent<RectTransform>());
               
                
            }
            else
            {
                DisablePlayer(collision.gameObject.GetComponent<PlayerManager>());
                StartCoroutine(LoadEndGame(3f, collision.gameObject.GetComponent<PlayerManager>()));
            }
        }
    }

    
    IEnumerator LoadEndGame(float second,PlayerManager player)
    {
        yield return new WaitForSeconds(second);
        player.gameObject.SetActive(false);
        GameManager.Instance.EndGame();
    }

   void DecreaseLive(RectTransform playerPosition)
    {
       
        if (!GameManager.Instance.GodMode)
        {
            GameManager.Instance.live -= 1;
            audioSource.PlayOneShot(hitsound);
            GameManager.Instance.lives[GameManager.Instance.live].SetActive(false);
            
        }
        else
        {
            GameManager.Instance.speed -= 100f;
            GameManager.Instance.ObsticalSpawnTime = (GameManager.Instance.ObsticalSpawnTime > 1) ? GameManager.Instance.ObsticalSpawnTime - 1f : 1f;
        }
        
        playerPosition.position = new Vector3(playerPosition.position.x,Screen.height/ 2f,0f);
                
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
