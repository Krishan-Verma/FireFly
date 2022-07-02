using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static Rigidbody2D rb;
    public float Upforce;
    public float Downforce;
    PlayerAction playerAction;
    public AudioSource audioSource;
    public Animator animator;
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
    }

    private void OnDisable()
    {
        playerAction.Player.Jump.performed -= Jump;
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
       
        rb.AddForce(new Vector2(0f, Upforce*jumprefactor),ForceMode2D.Impulse);
        
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
