using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody playerController;
    [SerializeField]private Animator animator;
    [SerializeField]private FixedJoystick joystick;
    Vector3 playerVelocity;
    public float moveSpeed = 2f;
    public float jumpHeight = 1f;
    float gravity = -9.81f;
    bool isGrounded;
    private void Start()
    {
        playerController = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        /*if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }*/
            
        playerController.velocity= new Vector3(joystick.Horizontal* moveSpeed, 0, joystick.Vertical* moveSpeed);
        if (joystick.Horizontal!= 0f||joystick.Vertical!= 0f)
        {            
            animator.SetBool("run",true);
        }
        else
        {
            animator.SetBool("run",false);
        }
        
    }
}
