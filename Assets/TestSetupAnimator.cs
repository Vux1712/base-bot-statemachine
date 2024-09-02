using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSetupAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator _animator;
    void Start()
    {
        _animator=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _animator.SetBool("walking",true);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            _animator.SetBool("walking",false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _animator.SetBool("running",true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _animator.SetBool("running",false);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            _animator.SetBool("fire",true);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            _animator.SetBool("fire",false);
        }
    }
}
