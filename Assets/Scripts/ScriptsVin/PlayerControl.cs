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
    private float speedChangeCD = 0f;                 // Ĭ���ٶȱ仯ʱ��
    [SerializeField]
    private float speedChangeRate = 1f;          // �ٶȱ仯��
    private float speedChangeDeviation = 0.01f; // �仯���ȶ��ٶ�����������ֵ��0.01f �ͺ� 
    [SerializeField]
    private float superJumpHeight; // ���������λ��
    [SerializeField]
    private float superJumpLength; // ���������γ�̾���
    [SerializeField]
    private float superJumpSpeed;  // ���������γ���ٶ�
    [SerializeField]
    private float fallAddition, jumpAddition; // ��غ������������ӳ�
    [SerializeField]
    private float levelStart, levelHeight; // ��ײ�߶Ⱥ�ÿ��߶�
    [SerializeField]
    private GameObject ghostPrefab;   // ����Ԥ����

    // ���
    private Rigidbody2D rb;                // ����
    private SpriteRenderer sr;             // ������Ⱦ��
    private Animator anim;                 // ����������
    [SerializeField]
    private ParticleSystem dustVFX;   // �ŵ׳���������Ч 
    [SerializeField]
    private Transform groundCheck;  // �������
    [SerializeField]
    private LayerMask groundLayer;  // ����Layer
    [SerializeField]
    private LayerMask layerDown, layerLevel1, layerLevel2, layerLevel3, layerLevel4, layerUp; // �㼶Layer
    private Animator curSpringAni;
    [SerializeField]
    private GhostRecorder gr;            // ������Ӱ��

    // ��ǰ״̬����
    private bool isGround;                      // �Ƿ��ڵ�����  
    private bool jumpHold;                     // �Ƿ��ڳ�����Ծ
    private int curDirection;                    // ��ǰ����
    private float curSpeed;                      // ��ǰ�ٶ�
    private int curSpeedState;                 // ��ǰ�ٶ�״̬��-1Ϊ���٣�0Ϊ������1Ϊ����
    private float curSpeedChangeCD;     // ��ǰ�ٶȱ仯ʱ�䵹��ʱ��С��0ʱ�ָ�ԭ�٣����õ���ʱ
    private float superJumpStart;            // �����������߶ȡ������㡾���á� 
    private int superJumpingState;         // ������״̬��0Ϊ�ǳ�������1Ϊ������������2Ϊ��������� 
    private bool canSpawn;                     // �Ƿ�������������
    private bool playerControl;               // ��ҿ���
    [SerializeField]
    private int curKeys;                           // Կ������

    private void Start()
    {
        // ��ȡ�������
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gr = GetComponent<GhostRecorder>(); 

        // ��Ҫ�ı�����ʼ��
        // �ٶȡ������ٶ�״̬�ĳ�ʼ��
        curDirection = 1;  // 1 == dash right, -1 == dash left 
        curSpeed = dashSpeed; 
        curSpeedState = 0;
        superJumpingState = 0;
        canSpawn = false;
        playerControl = true;
        curKeys = 0; 
    }

    private void Update()
    {
        if (!playerControl) return; 
         
        LapCheck();       // ��ͬȦ����������
        LevelCheck();     // ���㵱ǰ����
        FlipMe();            // ��ת���
        GroundCheck(); // ������
        Jump();              // ��Ծ     
        SpeedCheck();   // �ٶȵ����Ա仯
    }

    private void LapCheck()
    {
        GameObject _ghost;
        if (canSpawn)
        {
            switch (GameManager.GM.curLap)
            {
                case 2:
                    _ghost = Instantiate(ghostPrefab, transform.position, Quaternion.identity);
                    _ghost.GetComponent<GhostActor>().recorder = gr;
                    canSpawn = false; 
                    break;
                case 3:
                    _ghost = Instantiate(ghostPrefab, transform.position, Quaternion.identity);
                    _ghost.GetComponent<GhostActor>().recorder = gr;
                    canSpawn = false; 
                    break;
                case 4:
                    playerControl = false;
                    rb.velocity = Vector2.zero; 
                    break;
                default:
                    break;
            }
        }
    }
    private void LevelCheck()
    {
        // ������ײ��ȷ������
        if (Physics2D.OverlapCircle(transform.position, 0.1f, layerDown))
        {
            GameManager.GM.curLevel = 0; 
        }
        if (Physics2D.OverlapCircle(transform.position, 0.1f, layerLevel1))
        {
            GameManager.GM.curLevel = 1;
        }
        if (Physics2D.OverlapCircle(transform.position, 0.1f, layerLevel2))
        {
            GameManager.GM.curLevel = 2;
        }
        if (Physics2D.OverlapCircle(transform.position, 0.1f, layerLevel3))
        {
            GameManager.GM.curLevel = 3;
        }
        if (Physics2D.OverlapCircle(transform.position, 0.1f, layerLevel4))
        {
            GameManager.GM.curLevel = 4;
        }
        if (Physics2D.OverlapCircle(transform.position, 0.1f, layerUp))
        {
            GameManager.GM.curLevel = 5; 
        }

        /* �߶ȷ�������
        
        if (transform.position.y <= levelStart)
        {
            GameManager.GM.curLevel = 0; 
        }
        else if (transform.position.y >= levelStart + levelHeight * 4)
        {
            GameManager.GM.curLevel = 5; 
        }
        else
        {
            GameManager.GM.curLevel = (int)((transform.position.y - levelStart) / levelHeight) + 1;
        }

        */
    }
    private void GroundCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);  
    }
    private void Jump()
    {
        // if (isGround && Input.GetButtonDown("Jump"))
        if (Input.GetButtonDown("Jump")) // �����ã�����
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
                    curSpeed = Mathf.Lerp(curSpeed, dashSpeedSlow, speedChangeRate);
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
        if (!playerControl) return; 

        BasicDash();               // ������̵��ٶȿ���
        SuperJump();             // ��������������ٶȿ���
        JumpOptimization(); // ��������ص��ٶ�΢��
    }

    private void BasicDash()
    {
        rb.velocity = new Vector2(curSpeed * curDirection * Time.fixedDeltaTime, rb.velocity.y);
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

    // ��ת��ɫ����
    private void FlipMe()
    {
        var _flipValue = GameManager.GM.curLevel % 2; 
        switch (_flipValue) 
        {
            case 0: 
                FlipMeToRight(); 
                break; 
            case 1: 
                FlipMeToLeft(); 
                break; 
            default: 
                break; 
        }
    }
    private void FlipMeToRight()
    {
        curDirection = 1;
        sr.flipX = false; 
    }
    private void FlipMeToLeft()
    {
        curDirection = -1;
        sr.flipX = true; 
    }

    #region ��ײ�ʹ��� Collision
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
            curSpringAni = collision.collider.gameObject.GetComponent<Animator>();
            SoundManager.SM.PlayTrampoline(); 
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

            SoundManager.SM.PlaySpeedUp(); 

            // Destroy(collision.gameObject); 
        }

        // ����
        if (collision.tag == "SlowDown")
        {
            curSpeed = Mathf.Lerp(curSpeed, dashSpeedSlow, speedChangeRate); 
            curSpeedChangeCD = speedChangeCD; 
            curSpeedState = -1; 
        }

        // Ȧ����¼
        if (collision.tag == "LapCheck")
        {
            GameManager.GM.curLap++;
            canSpawn = true; 
        }

        // ײ�Ž���
        if (collision.tag == "Door")
        {
            if (curKeys == 3)
            {
                GameManager.GM.ShowGameDone(); 
            }
        }

    }
    // ���봥����
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Key")
        {
            curKeys++; 
            Destroy(collision.gameObject); 
        }
    }
    #endregion

    private void StartSuperJump()
    {
        superJumpingState = 1;
        superJumpStart = transform.position.y;
        rb.velocity = Vector2.up * superJumpForce;

        curSpringAni.SetBool("isSpring", true); 
    }

}
