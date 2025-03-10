using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rigidBody;
    private Vector2 moveDirection;
    private Animator animator;

    // Update is called once per frame
    void Update()
    {
        InputProcessor();

    }

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        PlayerMovement();
    }
    public void InputProcessor()
    { 
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY);

        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            animator.SetFloat("X", moveDirection.x);
            animator.SetFloat("Y", moveDirection.y);

            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
    public void PlayerMovement()
    {
        rigidBody.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    




}
