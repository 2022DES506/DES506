using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float bounceValue, greatJumpValue;  

    private bool isGround; 

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    private bool isJumping;

    private int curDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        curDirection = 1;  // 1 == dash right, -1 == dash left 
    }

    private void Update()
    {
        GetInput(); 
    }

    private void GetInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true; 
        }
    }

    private void FixedUpdate()
    {
        BasicDash();
        Jump(); 
    }

    private void BasicDash()
    {
        if (isGround && !isJumping)
        {
            rb.velocity = new Vector2(dashSpeed * curDirection, 0); 
            anim.SetBool("isRun", true); 
        }

    }

    private void Jump()
    {
        if (isJumping && isGround)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); 
            anim.SetBool("isJump", true); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGround = true;
            isJumping = false; 

            if (anim.GetBool("isJump"))
            {
                anim.SetBool("isJump", false); 
            }
        }

        if (collision.collider.tag == "Wall")
        {
            GoHigherLayer(); 
        }

        if (collision.collider.tag == "Portal")
        {
            transform.position = new Vector2(-5, 0); 
        }

        if (collision.collider.tag == "Spring")
        {
            GoHigherLayer(); 
        }

    }

    private void GoHigherLayer()
    {
        // 参数调整
        GameManager.GM.curLevel++;
        curDirection = -curDirection;
        sr.flipX = !sr.flipX;

        // 贴墙超级跳
        isJumping = true; 
        Vector2 newForce = Vector2.up;
        newForce.x = curDirection * bounceValue;
        newForce.y = Vector2.up.y * jumpForce * greatJumpValue;
        rb.AddForce(newForce, ForceMode2D.Impulse);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGround = false; 
        }

        if (collision.collider.tag == "Obstacle")
        {
            rb.velocity = new Vector2(curDirection * bounceValue, jumpForce); 
        }
    }
}
