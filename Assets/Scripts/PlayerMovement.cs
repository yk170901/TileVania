using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidBody;
    float gravity;
    CapsuleCollider2D myCollider;
    
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _jumpSpeed = 5f;
    [SerializeField] private float _climbSpeed = 5f;
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<CapsuleCollider2D>();
        gravity = myRigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    private void ClimbLadder()
    {
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) //  && Mathf.Abs(moveInput.y) > Mathf.Epsilon
        {
            Debug.Log("IOs touving ladder");
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, moveInput.y * _climbSpeed);
            myRigidBody.gravityScale = 0;
            myAnimator.SetBool("isCliming", Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon);
        }
        else
        {
            myRigidBody.gravityScale = gravity;
        }
    }

    private void FlipSprite()
    {
        bool isHorizontallyMoving = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

        if (isHorizontallyMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * _moveSpeed, myRigidBody.velocity.y); // y = 0 so that the player cannot fly
        myRigidBody.velocity = playerVelocity;

        myAnimator.SetBool("isRunning", Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) //  && Mathf.Abs(moveInput.y) > Mathf.Epsilon
        {
            Debug.Log("IOs asdasd ladder");
            myRigidBody.velocity += new Vector2(0f, moveInput.y);
        }
    }

    void OnJump(InputValue value)
    {
        if(value.isPressed && myCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myRigidBody.velocity += new Vector2(0f, _jumpSpeed);
        }
    }
}
