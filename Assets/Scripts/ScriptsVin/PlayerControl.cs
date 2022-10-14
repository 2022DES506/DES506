using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // �ɵ��ر���
    [SerializeField]
    private float dashSpeed, dashSpeedFast, dashSpeedSlow; // �����ٶ�
    [SerializeField]
    private float jumpForce, superJumpForce; // ������
    [SerializeField]
    private float speedChangeCD = 2f;                 // Ĭ���ٶȱ仯ʱ��
    [SerializeField]
    private float speedChangeRate = 0.01f;          // �ٶȱ仯��
    private float speedChangeDeviation = 0.01f; // �仯���ȶ��ٶ�����������ֵ��0.01f �ͺ� 
    [SerializeField]
    private float superJumpHeight; // ���������λ��
    [SerializeField]
    private float superJumpLength; // ���������γ�̾���
    [SerializeField]
    private float superJumpSpeed;  // ���������γ���ٶ�
    
    [SerializeField]
    private float fallAddition, jumpAddition; // ��غ������������ӳ�

    // ���
    private Rigidbody2D rb;              // ����
    private SpriteRenderer sr;           // ������Ⱦ��
    private Animator anim;               // ����������
    [SerializeField]
    private ParticleSystem dustVFX; // �ŵ׳���������Ч 

    // ��ǰ״̬����
    private bool isGround;                      // �Ƿ��ڵ�����  
    private bool jumpHold;                     // �Ƿ��ڳ�����Ծ
    private int curDirection;                    // ��ǰ����
    private float curSpeed;                      // ��ǰ�ٶ�
    private int curSpeedState;                 // ��ǰ�ٶ�״̬��-1Ϊ���٣�0Ϊ������1Ϊ����
    private float curSpeedChangeCD;     // ��ǰ�ٶȱ仯ʱ�䵹��ʱ��С��0ʱ�ָ�ԭ�٣����õ���ʱ
    private float superJumpStart;            // �����������߶ȡ������㡾���á� 
    private int superJumpingState;         // ������״̬��0Ϊ�ǳ�������1Ϊ������������2Ϊ��������� 

    private void Start()
    {
        // ��ȡ�������
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // ��Ҫ�ı�����ʼ��
        // �ٶȡ������ٶ�״̬�ĳ�ʼ��
        curDirection = 1;  // 1 == dash right, -1 == dash left 
        curSpeed = dashSpeed; 
        curSpeedState = 0;
        superJumpingState = 0; 

    }

    private void Update()
    {
        Jump();             // ��Ծ     
        SpeedCheck();  // �ٶȵ����Ա仯
    }

    private void Jump()
    {
        if (isGround && Input.GetButtonDown("Jump"))
        {
            // �ڵ����ϰ�����Ծһ˲��
            rb.velocity += Vector2.up * jumpForce; // �����ٶ�
            dustVFX.Play();                                        // �ﳾ��Ч
            SoundManager.SM.PlayJump();              // ��Ծ��Ч
        }
        jumpHold = Input.GetButton("Jump");       // ���������������
    }
    private void SpeedCheck()
    {
        curSpeedChangeCD -= Time.deltaTime; 
        if (curSpeedChangeCD <= 0)
        {
            // ����ʱ��������ٶ���������
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
            // ����ʱδ����ʱ�������ٶ�״̬���Ըı䵱ǰ�ٶ�ֵ
            switch (curSpeedState)
            {
                case -1:
                    // ����
                    curSpeed = Mathf.Lerp(dashSpeedSlow, curSpeed, 1 - speedChangeRate);
                    if (Mathf.Abs(curSpeed - dashSpeedSlow) < speedChangeDeviation)
                    {
                        curSpeed = dashSpeedSlow;
                    }
                    break;
                case 0:
                    // �����٣���ʵûɶ��Ҫ
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
                    // ����
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
        BasicDash();               // ������̵��ٶȿ���
        SuperJump();             // ��������������ٶȿ���
        JumpOptimization(); // ��������ص��ٶ�΢��
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
                // ����������
                float _jumpHeight = transform.position.y - superJumpStart;
                if (_jumpHeight >= superJumpHeight)
                {
                    // ������ߵ�
                    rb.gravityScale = 0f; // ȡ����������
                    superJumpStart = transform.position.x; // ��¼������
                    rb.velocity = new Vector2(superJumpSpeed * curDirection * Time.fixedDeltaTime, 0); // ��ʼ���
                    superJumpingState = 2; // ���³�����״̬
                    FlipMe(); // ��ת������

                    dustVFX.Play(); // ���Žŵ׳�����Ч
                }
                break;
            case 2:
                // ���������
                rb.velocity = new Vector2(superJumpSpeed * curDirection * Time.fixedDeltaTime, 0); // �������
                float _jumpLength = Mathf.Abs(transform.position.x - superJumpStart);
                if (_jumpLength >= superJumpLength)
                {
                    // ������յ�
                    rb.gravityScale = 1f; // �ָ���������
                    rb.velocity = new Vector2(curSpeed * curDirection * Time.fixedDeltaTime, rb.velocity.y); // �ٶȻָ�
                    superJumpingState = 0; // ���ó�����״̬
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
            // ���ʱ���ٶ����� 
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallAddition - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !jumpHold) 
        {
            // ����ʱ���ٶ����ƣ������򲻴�������
            rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpAddition - 1) * Time.fixedDeltaTime; 
        }
    }
    // ������߲�
    private void GotHigherLayer()
    {
        GameManager.GM.curLevel++;
    }
    // ��ת��ɫ����
    private void FlipMe()
    {
        curDirection = -curDirection;
        sr.flipX = !sr.flipX; 
    }

    #region ��ײ�ʹ���
    // ������ײ��
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
    // ������ײ��
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGround = false; 
        }
    }

    // ����������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ����
        if (collision.tag == "SpeedUp")
        {
            GameManager.GM.curCoins++; 

            curSpeed = Mathf.Lerp(curSpeed, dashSpeedFast, speedChangeRate); 
            curSpeedChangeCD = speedChangeCD; 
            curSpeedState = 1; 

            Destroy(collision.gameObject); 
        }

        // ����
        if (collision.tag == "SlowDown")
        {
            curSpeed = Mathf.Lerp(dashSpeedSlow, curSpeed, 1-speedChangeRate); 
            curSpeedChangeCD = speedChangeCD; 
            curSpeedState = -1; 
        }

    }
    // ���봥����
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
