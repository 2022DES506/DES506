using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // 可调控变量
    [SerializeField]
    private float dashSpeed, dashSpeedFast, dashSpeedSlow; 
    [SerializeField]
    private float jumpForce; 
    [SerializeField]
    private float bounceValue, greatJumpValue;
    [SerializeField]
    private float defaultTimer;
    [SerializeField]
    private float speedChangeTime, speedChangeRate;
    [SerializeField]
    private float superJumpHeight;
    [SerializeField]
    private ParticleSystem dustVFX; 

    // 状态判断
    private bool isGround;
    private bool isJumping;
    private bool isSuperJumping; 

    // 组件
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    // 当前状态变量
    private int curDirection;
    private float curSpeed;
    private int curSpeedState; 
    private float curTimer;
    private float superJumpStart; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        curDirection = 1;  // 1 == dash right, -1 == dash left 
        curSpeed = dashSpeed;
        curSpeedState = 0; 

    }

    private void Update()
    {
        GetInput();
        SpeedCheck(); 
    }

    private void SpeedCheck()
    {
        curTimer -= Time.deltaTime; 
        if (curTimer <= 0)
        {
            if (curSpeed != dashSpeed)
            {
                curSpeed = Mathf.Lerp(curSpeed, dashSpeed, speedChangeRate);
                if (Mathf.Abs(curSpeed - dashSpeed) < 0.01f)
                {
                    curSpeed = dashSpeed; 
                }
            }
        }
        else
        {
            switch (curSpeedState)
            {
                case -1:
                    curSpeed = Mathf.Lerp(dashSpeedSlow, curSpeed, 1 - speedChangeRate);
                    if (Mathf.Abs(curSpeed - dashSpeedSlow) < 0.01f)
                    {
                        curSpeed = dashSpeedSlow;
                    }
                    break;
                case 0:
                    curSpeed = Mathf.Lerp(curSpeed, dashSpeed, speedChangeRate);
                    if (Mathf.Abs(curSpeed - dashSpeed) < 0.01f)
                    {
                        curSpeed = dashSpeed;
                    }
                    break;
                case 1:
                    curSpeed = Mathf.Lerp(curSpeed, dashSpeedFast, speedChangeRate);
                    if (Mathf.Abs(curSpeed - dashSpeedFast) < 0.01f)
                    {
                        curSpeed = dashSpeedFast; 
                    }
                    break;
            }
        }
        
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
        SuperJump();
    }

    private void SuperJump()
    {
        if (isSuperJumping)
        {
            float jumpHeight = transform.position.y - superJumpStart; 
            if (jumpHeight >= superJumpHeight)
            {
                rb.AddForce(Vector2.right * curDirection * 2, ForceMode2D.Impulse);
                isSuperJumping = false;

                dustVFX.Play(); 
            }
        }
    }

    private void BasicDash()
    {
        if (isGround && !isJumping)
        {
            rb.velocity = new Vector2(curSpeed * curDirection, 0); 
        }

    }

    private void Jump()
    {
        if (isJumping && isGround)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); 
            anim.SetBool("isJump", true);
            dustVFX.Play(); 
        }
    }

    #region 碰撞和触发
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGround = true;
            isJumping = false;

            dustVFX.Play(); 

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
            GameManager.GM.curLap++; 
        }

        if (collision.collider.tag == "Spring")
        {
            GoHigherLayer(); 
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGround = false; 
        }

        if (collision.collider.tag == "Obstacle")
        {
            rb.velocity = new Vector2(curDirection * bounceValue, jumpForce  * greatJumpValue); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SpeedUp")
        {
            GameManager.GM.curCoins++; 

            curSpeed = Mathf.Lerp(curSpeed, dashSpeedFast, speedChangeRate); 
            curTimer = defaultTimer; 
            curSpeedState = 1; 

            Destroy(collision.gameObject); 
        }

        if (collision.tag == "SlowDown")
        {
            curSpeed = Mathf.Lerp(dashSpeedSlow, curSpeed, 1-speedChangeRate); 
            curTimer = defaultTimer; 
            curSpeedState = -1; 
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "Hollow")
        {
            GameManager.GM.curLevel--; 
            curDirection = -curDirection;
            sr.flipX = !sr.flipX; 
        }
    }
    #endregion

    private void GoHigherLayer()
    {
        // 参数调整
        GameManager.GM.curLevel++;
        curDirection = -curDirection;
        sr.flipX = !sr.flipX;
        rb.velocity = Vector2.zero; 

        // 超级跳
        isJumping = true;
        isSuperJumping = true;
        superJumpStart = transform.position.y;  
        Vector2 newForce = Vector2.up;
        newForce.y = Vector2.up.y * jumpForce * greatJumpValue * 2;
        rb.AddForce(newForce, ForceMode2D.Impulse); 
    }

}
