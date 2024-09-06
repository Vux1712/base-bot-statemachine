using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController playerController;
    [SerializeField]private Animator animator;
    Vector3 playerVelocity;
    public float moveSpeed = 2f;
    public float jumpHeight = 1f;
    float gravity = -9.81f;
    bool isGrounded;
    private void Start()
    {
        playerController = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = playerController.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
            
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Horizontal"));
        /*if (/*direction.magnitude#1#Input.GetAxis("Horizontal") >= 0.1f||Input.GetAxis("Horizontal")>=0.1f)
        {            
            animator.SetBool("walk",true);
            animator.SetBool("idle",false);
        }
        else
        {
            animator.SetBool("walk",false);
            animator.SetBool("idle",true);
        }*/
        playerController.Move(direction * Time.deltaTime * moveSpeed);
        
        Quaternion playerRotation = Quaternion.LookRotation(direction);
        transform.rotation =Quaternion.Slerp(transform.rotation,playerRotation,Time.deltaTime * 10f);
    }
}
