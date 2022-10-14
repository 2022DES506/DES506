using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // 可调控变量
    [SerializeField]
    private float dashSpeed, dashSpeedFast, dashSpeedSlow; // 三种速度
    [SerializeField]
    private float jumpForce, superJumpForce; // 弹跳力
    [SerializeField]
    private float speedChangeCD = 2f;                 // 默认速度变化时间
    [SerializeField]
    private float speedChangeRate = 0.01f;          // 速度变化率
    private float speedChangeDeviation = 0.01f; // 变化至既定速度所允许的误差值，0.01f 就好 
    [SerializeField]
    private float superJumpHeight; // 超级跳冲刺位置
    [SerializeField]
    private float superJumpLength; // 超级跳后半段冲刺距离
    [SerializeField]
    private float superJumpSpeed;  // 超级跳后半段冲刺速度
    
    [SerializeField]
    private float fallAddition, jumpAddition; // 落地和起跳的重力加成

    // 组件
    private Rigidbody2D rb;              // 刚体
    private SpriteRenderer sr;           // 精灵渲染器
    private Animator anim;               // 动画控制器
    [SerializeField]
    private ParticleSystem dustVFX; // 脚底尘埃粒子特效 

    // 当前状态变量
    private bool isGround;                      // 是否在地面上  
    private bool jumpHold;                     // 是否在长按跳跃
    private int curDirection;                    // 当前方向
    private float curSpeed;                      // 当前速度
    private int curSpeedState;                 // 当前速度状态：-1为减速，0为正常，1为加速
    private float curSpeedChangeCD;     // 当前速度变化时间倒计时，小于0时恢复原速，重置倒计时
    private float superJumpStart;            // 超级跳起跳高度、冲刺起点【复用】 
    private int superJumpingState;         // 超级跳状态：0为非超级跳，1为超级跳起跳，2为超级跳冲刺 

    private void Start()
    {
        // 获取自身组件
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // 必要的变量初始化
        // 速度、方向、速度状态的初始化
        curDirection = 1;  // 1 == dash right, -1 == dash left 
        curSpeed = dashSpeed; 
        curSpeedState = 0;
        superJumpingState = 0; 

    }

    private void Update()
    {
        Jump();             // 跳跃     
        SpeedCheck();  // 速度的线性变化
    }

    private void Jump()
    {
        if (isGround && Input.GetButtonDown("Jump"))
        {
            // 在地面上按下跳跃一瞬间
            rb.velocity += Vector2.up * jumpForce; // 起跳速度
            dustVFX.Play();                                        // 扬尘特效
            SoundManager.SM.PlayJump();              // 跳跃音效
        }
        jumpHold = Input.GetButton("Jump");       // 长按解除起跳抑制
    }
    private void SpeedCheck()
    {
        curSpeedChangeCD -= Time.deltaTime; 
        if (curSpeedChangeCD <= 0)
        {
            // 倒计时结束后的速度线性重置
            if (curSpeed != dashSpeed)
            {
                curSpeed = Mathf.Lerp(curSpeed, dashSpeed, speedChangeRate);
                if (Mathf.Abs(curSpeed - dashSpeed) < speedChangeDeviation)
                {
                    curSpeed = dashSpeed; 
                }
            }
        }
        else
        {
            // 倒计时未结束时，根据速度状态线性改变当前速度值
            switch (curSpeedState)
            {
                case -1:
                    // 减速
                    curSpeed = Mathf.Lerp(dashSpeedSlow, curSpeed, 1 - speedChangeRate);
                    if (Mathf.Abs(curSpeed - dashSpeedSlow) < speedChangeDeviation)
                    {
                        curSpeed = dashSpeedSlow;
                    }
                    break;
                case 0:
                    // 正常速，其实没啥必要
                    if (curSpeed != dashSpeed)
                    {
                        curSpeed = Mathf.Lerp(curSpeed, dashSpeed, speedChangeRate);
                        if (Mathf.Abs(curSpeed - dashSpeed) < speedChangeDeviation)
                        {
                            curSpeed = dashSpeed;
                        }
                    }
                    break;
                case 1:
                    // 加速
                    curSpeed = Mathf.Lerp(curSpeed, dashSpeedFast, speedChangeRate);
                    if (Mathf.Abs(curSpeed - dashSpeedFast) < speedChangeDeviation)
                    {
                        curSpeed = dashSpeedFast; 
                    }
                    break;
            }
        }
        
    }

    private void FixedUpdate()
    {
        BasicDash();               // 正常冲刺的速度控制
        SuperJump();             // 超级跳触发后的速度控制
        JumpOptimization(); // 起跳与落地的速度微调
    }

    private void BasicDash()
    {
        if (isGround)
        {
            rb.velocity = new Vector2(curSpeed * curDirection * Time.fixedDeltaTime, rb.velocity.y);
        }

    }
    private void SuperJump()
    {
        switch (superJumpingState)
        {
            case 1:
                // 超级跳起跳
                float _jumpHeight = transform.position.y - superJumpStart;
                if (_jumpHeight >= superJumpHeight)
                {
                    // 跳至最高点
                    rb.gravityScale = 0f; // 取消重力悬空
                    superJumpStart = transform.position.x; // 记录冲刺起点
                    rb.velocity = new Vector2(superJumpSpeed * curDirection * Time.fixedDeltaTime, 0); // 开始冲刺
                    superJumpingState = 2; // 更新超级跳状态
                    FlipMe(); // 翻转自身方向

                    dustVFX.Play(); // 播放脚底尘埃特效
                }
                break;
            case 2:
                // 超级跳冲刺
                rb.velocity = new Vector2(superJumpSpeed * curDirection * Time.fixedDeltaTime, 0); // 持续冲刺
                float _jumpLength = Mathf.Abs(transform.position.x - superJumpStart);
                if (_jumpLength >= superJumpLength)
                {
                    // 冲刺至终点
                    rb.gravityScale = 1f; // 恢复刚体重力
                    rb.velocity = new Vector2(curSpeed * curDirection * Time.fixedDeltaTime, rb.velocity.y); // 速度恢复
                    superJumpingState = 0; // 重置超级跳状态
                    GotHigherLayer(); 
                }
                break;
            default:
                break;
        }
    }
    private void JumpOptimization()
    {
        if (rb.velocity.y < 0)
        {
            // 落地时的速度增幅 
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallAddition - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !jumpHold) 
        {
            // 起跳时的速度抑制，长按则不触发抑制
            rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpAddition - 1) * Time.fixedDeltaTime; 
        }
    }
    // 到达更高层
    private void GotHigherLayer()
    {
        GameManager.GM.curLevel++;
    }
    // 翻转角色朝向
    private void FlipMe()
    {
        curDirection = -curDirection;
        sr.flipX = !sr.flipX; 
    }

    #region 碰撞和触发
    // 触碰碰撞体
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGround = true;

            dustVFX.Play(); 

            if (anim.GetBool("isJump"))
            {
                anim.SetBool("isJump", false); 
            }
        }

        if (collision.collider.tag == "Spring")
        {
            StartSuperJump(); 
        }

    }
    // 脱离碰撞体
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGround = false; 
        }
    }

    // 触碰触发体
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 加速
        if (collision.tag == "SpeedUp")
        {
            GameManager.GM.curCoins++; 

            curSpeed = Mathf.Lerp(curSpeed, dashSpeedFast, speedChangeRate); 
            curSpeedChangeCD = speedChangeCD; 
            curSpeedState = 1; 

            Destroy(collision.gameObject); 
        }

        // 减速
        if (collision.tag == "SlowDown")
        {
            curSpeed = Mathf.Lerp(dashSpeedSlow, curSpeed, 1-speedChangeRate); 
            curSpeedChangeCD = speedChangeCD; 
            curSpeedState = -1; 
        }

    }
    // 脱离触发体
    private void OnTriggerExit2D(Collider2D collision)
    {

    }
    #endregion

    private void StartSuperJump()
    {
        superJumpingState = 1;
        superJumpStart = transform.position.y;
        rb.velocity = Vector2.up * superJumpForce; 
    }

}
