using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheildManager : MonoBehaviour
{ 
    public AudioClip SheildGain;
    public float sheildDuration=10f;

    int sheildIndex = 3;
    private void Awake()
    {
        sheildDuration = GameManager.Instance.UpdatePowerUpsDuration(sheildIndex);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GodMode = true;
            GameManager.Instance.audioSource.PlayOneShot(SheildGain);
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            gameObject.transform.SetParent(collision.transform,false);
            gameObject.GetComponent<RectTransform>().position=collision.gameObject.GetComponent<RectTransform>().position;
            Destroy(gameObject, sheildDuration);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.GodMode = false;

    }
}
