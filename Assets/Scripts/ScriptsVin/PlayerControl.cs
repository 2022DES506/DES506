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

    // [SerializeField] 累加变速
    // private float speedChangeTimer, speedChangeValue; 

    // 状态判断
    private bool isGround;
    private bool isJumping;

    // 组件
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    // 当前状态变量
    private int curDirection;
    private float curSpeed;
    private int curSpeedState; 
    private float curTimer; 

    /* 累加变速
    private struct speedChange
    {
        public float curTimer;
        public float curValue; 
    }
    private List<speedChange> sc;
    private bool isSlowDown;
    private float totalChangeValue; 
    */

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        curDirection = 1;  // 1 == dash right, -1 == dash left 
        curSpeed = dashSpeed;
        curSpeedState = 0; 

        // sc = new List<speedChange>(); // 累加变速
    }

    private void Update()
    {
        GetInput();
        // SpeedChangeCheck(); // 累加变速
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

    /* 累加变速
    private void SpeedChangeCheck()
    {
        if (sc==null || sc.Count == 0) return; 

        for (int i=0; i<sc.Count; i++)
        {
            if (sc[i].curTimer <= 0)
            {
                sc.RemoveAt(i); 
            }
        }

        totalChangeValue = 0f; 
        for (int i=0; i<sc.Count; i++)
        {
            totalChangeValue += sc[i].curValue; 
        }

        for (int i=0; i<sc.Count; i++)
        {
            speedChange tempSC = sc[i];
            tempSC.curTimer -= Time.deltaTime;
            sc[i] = tempSC; 
        }
    }
    */

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
            // rb.velocity = new Vector2((dashSpeed + totalChangeValue)* curDirection, 0); 

            rb.velocity = new Vector2(curSpeed * curDirection, 0); 
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


    #region 碰撞和触发
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

        /* 累加变速
        if (collision.tag == "SpeedUp")
        {
            speedChange newChange;
            newChange.curTimer = speedChangeTimer;
            newChange.curValue = speedChangeValue;
            sc.Add(newChange);

            Destroy(collision.gameObject); 
        }

        if (collision.tag == "SlowDown")
        {
            if (isSlowDown == false)
            {
                speedChange newChange;
                newChange.curTimer = speedChangeTimer;
                newChange.curValue = -speedChangeValue;
                sc.Add(newChange); 

                isSlowDown = true; 
            }
        }
        */
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        /* 累加变速
        if (collision.tag == "SlowDown")
        {
            isSlowDown = false; 
        }
        */

        if (collision.tag == "Hollow")
        {
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

        // 贴墙超级跳
        isJumping = true;
        Vector2 newForce = Vector2.up;
        newForce.x = curDirection * bounceValue * 2;
        newForce.y = Vector2.up.y * jumpForce * greatJumpValue * 2;
        rb.AddForce(newForce, ForceMode2D.Impulse);
    }

}
