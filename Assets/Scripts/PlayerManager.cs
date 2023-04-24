using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    
    PlayerAction playerAction;

    public static Rigidbody2D rb;
    public AudioSource audioSource;
    public Animator animator;

    float Upforce = 175f;
    float Downforce = -150f;
    float jumprefactor;

    private void Awake()
    {
        jumprefactor = Screen.width / Screen.height;
        audioSource = GetComponent<AudioSource>();
        playerAction = new PlayerAction();
        
    }
    private void OnEnable()
    {
       
        playerAction.Player.Enable();
        playerAction.Player.Jump.performed += Jump;
        playerAction.Player.Exit.performed += Exit;
    }

  
    
    private void OnDisable()
    {
        playerAction.Player.Jump.performed -= Jump;
        playerAction.Player.Exit.performed -= Exit;
        playerAction.Player.Disable();
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        PlaneSound();
    }
    void Update()
    {
        
      rb.velocity += new Vector2(0f, Downforce*Time.deltaTime*jumprefactor);
        

    }

    public void Jump(InputAction.CallbackContext obj)
    {
       if(Time.timeScale!=0)
        rb.AddForce(new Vector2(0f, Upforce*jumprefactor),ForceMode2D.Impulse);
        
    }

    public void Exit(InputAction.CallbackContext obj)
    {
        GameManager.Instance.Pause();  
    }
    public void PlaneSound()
    {
        
        if (PlayerPrefs.GetString("PlaneSound")=="ON")
        {
            audioSource.Play(); 
        }
        else
        {
            audioSource.Pause();
        }
    }
}
