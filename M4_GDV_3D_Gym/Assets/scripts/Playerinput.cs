using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playerinput : MonoBehaviour
{

    [SerializeField] private InputActionAsset input;
    [SerializeField] private string actionMapName = "Player1";

    private InputActionMap map;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    [SerializeField] private CharacterController CC;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 5f;
    private Vector3 move;
    private float moveSpeed = 5f;
    private float verticalVelocity;
    private Animator animator;
    void Awake()
    {
    
        map = input.FindActionMap(actionMapName);
        moveAction = map.FindAction("Move");
        jumpAction = map.FindAction("Jump");
        sprintAction = map.FindAction("Sprint");
        animator = GetComponent<Animator>(); 
    }

    void OnEnable()
    {
        map.Enable();
    }

    void OnDisable()
    {
        map.Disable();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       CC= GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CC.isGrounded)
        {
            verticalVelocity = -1f; // kleine downward force om grounded te blijven

            if (jumpAction.WasPressedThisFrame())
             {
                verticalVelocity = Mathf.Sqrt(2f * Mathf.Abs(gravity) * jumpHeight); //hoeveel kracht moet je geven om op de juiste hoogte uit te komen?
                animator.SetBool("jump", true);
             }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // zwaartekracht trekt mij naar beneden
        }

            move.y = verticalVelocity * Time.deltaTime; //verticale berekening word meegegeven aan de move variabele
            CC.Move(move); //hier geven we de uiteindelijke move variabele mee aan de Move functie
       

        if (sprintAction.IsPressed())
        {
            moveSpeed = 10f;
            
        }
        else
        {
            moveSpeed = 5f;
        }
        
        
       
        
        
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        float currentMovespeed = moveInput.y * moveSpeed * Time.deltaTime;
       // transform.Translate(transform.forward * currentMovespeed, Space.World);
        transform.Rotate(Vector3.up, moveInput.x * Time.deltaTime * 100f);

      animator.SetFloat("Speed", currentMovespeed);
      animator.SetBool("jump", jumpAction.IsPressed());
       CC.Move(transform.forward * currentMovespeed );
    }

}