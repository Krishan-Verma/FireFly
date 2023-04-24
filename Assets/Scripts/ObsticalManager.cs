using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticalManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hitsound;

    public static ObsticalManager Instance;
    int minimumLive = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Instance = this;


    }

    void DisablePlayer(PlayerManager player)
    {

        player.tag = "Dead";
        player.animator.SetBool("IsDead", true);
        player.audioSource.Stop();
        player.GetComponent<Rigidbody2D>().gravityScale = 10f;
        player.enabled = false;

        GameManager.Instance.IsDead = true;
        GameManager.Instance.audioSource.Stop();
        GameManager.Instance.audioSource.PlayOneShot(GameManager.Instance.gameOverClip);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && !GameManager.Instance.Invisible(collision.gameObject.GetComponent<PlayerManager>()) && !GameManager.Instance.GodMode)
        {

            if (GameManager.Instance.live > minimumLive)
            {

                DecreaseLive(collision.gameObject.GetComponent<RectTransform>());


            }
            else
            {
                GameManager.Instance.ShowAdPanel(collision.gameObject.GetComponent<PlayerManager>());

            }

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && !GameManager.Instance.Invisible(collision.gameObject.GetComponent<PlayerManager>()) && !GameManager.Instance.GodMode)
        {



            if (GameManager.Instance.live > minimumLive)
            {

                DecreaseLive(collision.gameObject.GetComponent<RectTransform>());


            }
            else
            {
                GameManager.Instance.ShowAdPanel(collision.gameObject.GetComponent<PlayerManager>());


            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && !GameManager.Instance.Invisible(collision.gameObject.GetComponent<PlayerManager>())&& !GameManager.Instance.GodMode)
        {


            if (GameManager.Instance.live > minimumLive)
            {



                DecreaseLive(collision.gameObject.GetComponent<RectTransform>());


            }
            else
            {
                GameManager.Instance.ShowAdPanel(collision.gameObject.GetComponent<PlayerManager>());

            }
        }
    }


    IEnumerator LoadEndGame(float second, PlayerManager player)
    {
        yield return new WaitForSeconds(second);
        player.gameObject.SetActive(false);
        GameManager.Instance.EndGame();
    }

    void DecreaseLive(RectTransform playerPosition)
    {
        GameManager.Instance.live -= 1;
        audioSource.PlayOneShot(hitsound);
        GameManager.Instance.lives[GameManager.Instance.live].SetActive(false);
        playerPosition.position = new Vector3(playerPosition.position.x, Screen.height / 2f, 0f);

    }

    public void End(PlayerManager player)
    {
        if (player != null)
        {
            DisablePlayer(player);
            StartCoroutine(LoadEndGame(3f, player));
        }
    }

}


