using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private float speedChangeDeviation = 0.01f; // �仯���ȶ��ٶ������������ֵ��0.01f �ͺ� 
    [SerializeField]
    private float superJumpHeight; // ���������λ��
    [SerializeField]
    private float superJumpLength; // ���������γ�̾���
    [SerializeField]
    private float superJumpSpeed;  // ���������γ���ٶ�
    [SerializeField]
    private float fallAddition, jumpAddition; // ��غ������������ӳ�
    [SerializeField]
    private float jumpHeight, holdJumpHeight; // ������Ծ�߶ȿ���
    [SerializeField]
    private float levelStart, levelHeight; // ��ײ�߶Ⱥ�ÿ��߶�
    [SerializeField]
    private GameObject ghostPrefab, ghostPrefab2;   // ����Ԥ����
    [SerializeField]
    private float flyForce;                       // �������
    [SerializeField]
    private float flySpeed;                      // �����ٶ�
    [SerializeField]
    private float hDown, hRight1, hRight2, hRight3, hRight4, hUp, hLeft4, hLeft3, hLeft2, hLeft1;
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private float nextRingCD;
    [SerializeField]
    private float ringSpawnRange;
    [SerializeField]
    private float ringPosFix; 

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
    [SerializeField]
    private LayerMask BgcLayer;
    [SerializeField]
    private GameObject airPlane1, airPlane2, airPlane3;
    [SerializeField]
    private GameObject RingPrefab;  // ����ԲȦԤ����
    [SerializeField]
    private GameObject blackBG;      // ��ɫ����
    [SerializeField]
    private GameObject point;
    [SerializeField]
    private GameObject lap;
    [SerializeField]
    private AudioSource Portal;
    [SerializeField]
    private AudioSource Spike;
    [SerializeField]
    private AudioSource jump;
    [SerializeField]
    private Text pointText;

    // ��ǰ״̬����
    public bool isGround;                      // �Ƿ��ڵ�����  
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
    private bool isFlying;                        // �Ƿ��������
    public bool isJumping;                    // �Ƿ�����������
    private float curJumpStart;               // ��ǰ��Ծ�߶�
    private float speedBeforeFly;            // ��¼���������֮ǰ���ٶ�
    private float curRingTimer;
    public bool isSpeedUping; 

    private void Start()
    {
        // ��ȡ�������
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gr = GetComponent<GhostRecorder>();

        pointText.text = "";

        // ��Ҫ�ı�����ʼ��
        // �ٶȡ������ٶ�״̬�ĳ�ʼ��
        curDirection = 1;  // 1 == dash right, -1 == dash left 
        curSpeed = dashSpeed; 
        curSpeedState = 0;
        superJumpingState = 0;
        canSpawn = false;
        playerControl = false;
        curKeys = 0;
        isFlying = false;
        curRingTimer = nextRingCD; 

        Invoke("StartRun", 3f); // 3���ִ��StartRun
    }

    private void StartRun()
    {
        playerControl = true; 
    }

    private void Update()
    {
        if (!playerControl) return;

        BackgroundRing(); 
        // BackgroundChangeCheck(); 
        JumpHeightCheck(); // ������Ծ�߶�
        LapCheck();       // ��ͬȦ����������
        LevelCheck();     // ���㵱ǰ����
        LayerCheck();     // ���ݸ߶ȼ������
        FlipMe();            // ��ת���
        GroundCheck(); // ������
        Jump();              // ��Ծ     
        SpeedCheck();   // �ٶȵ����Ա仯
        DataToManager(); // ����������Ϣ��Manager

    }

    private void BackgroundRing()
    {
        curRingTimer -= Time.deltaTime; 
        if (curRingTimer <= 0 )
        {
            Vector2 _spawnPos = new Vector2(transform.position.x + curDirection * ringPosFix, transform.position.y) + Random.insideUnitCircle * ringSpawnRange;  
            Instantiate(RingPrefab, _spawnPos, Quaternion.identity);  
            curRingTimer = nextRingCD; 
        }
    }

    private void LayerCheck()
    {
        if (transform.position.y == hDown && GameManager.GM.curLayerState == GameManager.LayerState.layerLeft1)
        {
            GameManager.GM.curLayerState = GameManager.LayerState.layerDown;
            GameManager.GM.layerChangeLock = false; 
        }
        if (transform.position.y == hRight1 && GameManager.GM.curLayerState == GameManager.LayerState.layerDown)
        {
            GameManager.GM.curLayerState = GameManager.LayerState.layerRight1;
            GameManager.GM.layerChangeLock = false; 
        }
        if (transform.position.y == hRight2 && GameManager.GM.curLayerState == GameManager.LayerState.layerRight1)
        {
            GameManager.GM.curLayerState = GameManager.LayerState.layerRight2;
            GameManager.GM.layerChangeLock = false; 
        }
        if (transform.position.y == hRight3 && GameManager.GM.curLayerState == GameManager.LayerState.layerRight2)
        {
            GameManager.GM.curLayerState = GameManager.LayerState.layerRight3;
            GameManager.GM.layerChangeLock = false; 
        }
        if (transform.position.y == hRight4 && GameManager.GM.curLayerState == GameManager.LayerState.layerRight3)
        {
            GameManager.GM.curLayerState = GameManager.LayerState.layerRight4;
            GameManager.GM.layerChangeLock = false; 
        }
        if (transform.position.y == hUp && GameManager.GM.curLayerState == GameManager.LayerState.layerRight4) 
        {
            GameManager.GM.curLayerState = GameManager.LayerState.layerUp;
            GameManager.GM.layerChangeLock = false; 
        }
        if (transform.position.y == hLeft4 && GameManager.GM.curLayerState == GameManager.LayerState.layerUp)
        {
            GameManager.GM.curLayerState = GameManager.LayerState.layerLeft4;
            GameManager.GM.layerChangeLock = false; 
        }
        if (transform.position.y == hLeft3 && GameManager.GM.curLayerState == GameManager.LayerState.layerLeft4)
        {
            GameManager.GM.curLayerState = GameManager.LayerState.layerLeft3;
            GameManager.GM.layerChangeLock = false; 
        }
        if (transform.position.y == hLeft2 && GameManager.GM.curLayerState == GameManager.LayerState.layerLeft3)
        {
            GameManager.GM.curLayerState = GameManager.LayerState.layerLeft2;
            GameManager.GM.layerChangeLock = false; 
        }
        if (transform.position.y == hLeft1 && GameManager.GM.curLayerState == GameManager.LayerState.layerLeft2)
        {
            GameManager.GM.curLayerState = GameManager.LayerState.layerLeft1;
            GameManager.GM.layerChangeLock = false; 
        }
    }

    private void DataToManager()
    {
        GameManager.GM.playerPos = transform.position;
        GameManager.GM.playerDir = curDirection; 
    }
    private void JumpHeightCheck()
    {
        if (isJumping)
        {
            if (!jumpHold)
            {
                if (transform.position.y - curJumpStart >= jumpHeight)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    isJumping = !isJumping;
                }
            }
            else
            {
                if (transform.position.y - curJumpStart >= holdJumpHeight)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    isJumping = !isJumping; 
                }
            }
        }
    }
    private void LapCheck()
    {
        GameObject _ghost, _arrow;
        if (canSpawn)
        {
            switch (GameManager.GM.curLap)
            {
                // ���������ָʾ��ͷ
                case 2:
                    _ghost = Instantiate(ghostPrefab, transform.position, Quaternion.identity);
                    _ghost.GetComponent<GhostActor>().recorder = gr;
                    _arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity, transform);
                    _arrow.GetComponent<Arrow>().tarGhost = _ghost; 
                    canSpawn = false; 
                    break;
                case 3:
                    _ghost = Instantiate(ghostPrefab2, transform.position, Quaternion.identity);
                    _ghost.GetComponent<GhostActor>().recorder = gr; 
                    _arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity, transform);
                    _arrow.GetComponent<Arrow>().tarGhost = _ghost; 
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

    private void BackgroundChangeCheck()
    {
        if (Physics2D.OverlapCircle(groundCheck.position, 0.1f, BgcLayer))
        {
            GameManager.GM.layerChangeLock = false;
        }
        else
        {
            GameManager.GM.layerChangeLock = true;
        }
    }

    private void GroundCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);  
    }
    private void Jump()
    {
        if (isFlying)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity += Vector2.up * flyForce;      // �����ٶ�
                dustVFX.Play();                                        // �ﳾ��Ч
                jump.Play();             // ��Ծ��Ч
            }
        }
        else
        {
            if (isGround && Input.GetButtonDown("Jump"))
            {
                // �ڵ����ϰ�����Ծһ˲��
                rb.velocity += Vector2.up * jumpForce; // �����ٶ�
                dustVFX.Play();                                        // �ﳾ��Ч
                jump.Play();              // ��Ծ��Ч

                isJumping = true;
                curJumpStart = transform.position.y;  
            }
        }
        jumpHold = Input.GetButton("Jump");           // ���������������
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

            GameManager.GM.isSpeedUp = false;
            isSpeedUping = false;
            GameManager.GM.isSlowDown = false; 
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

                    PointsSystem.PS.AddPoints(500); 
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
            if (isFlying)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpAddition - 1) * Time.fixedDeltaTime; 
            }
            else
            {
                if (Mathf.Abs(rb.velocity.y) < 20f) 
                {
                    rb.velocity += Vector2.up * Physics2D.gravity.y * (fallAddition - 1) * Time.fixedDeltaTime;
                }
            }
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

            rb.velocity = new Vector2(rb.velocity.x, 0f); // ��ֹ�����ٶȹ�����ɵķ�����ģ

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
            PointsSystem.PS.AddPoints(100);
            PointsSystem.PS.curGFCoins++; 

            curSpeed = Mathf.Lerp(curSpeed, dashSpeedFast, speedChangeRate); 
            curSpeedChangeCD = speedChangeCD; 
            curSpeedState = 1;

            SoundManager.SM.PlaySpeedUp();

            GameManager.GM.isSpeedUp = true;
            isSpeedUping = true;
            GameManager.GM.isSlowDown = false; 

            point.GetComponent<Animator>().SetTrigger("Change");

            pointText.text = "Coin Collected";


            // Destroy(collision.gameObject); 
        }

        // ����
        if (collision.tag == "SlowDown")
        {
            curSpeed = Mathf.Lerp(curSpeed, dashSpeedSlow, speedChangeRate); 
            curSpeedChangeCD = speedChangeCD; 
            curSpeedState = -1;

            Spike.Play();

            GameManager.GM.isSpeedUp = false;
            isSpeedUping = false;
            GameManager.GM.isSlowDown = true;
            PointsSystem.PS.ResetCFTimer(); 
            pointText.text = "Hit Spike";
        }

        // Ȧ����¼
        if (collision.tag == "LapCheck")
        {
            GameManager.GM.curLap++;
            canSpawn = true;

            if (GameManager.GM.curLap == 2)
            {
                anim.SetBool("isTwo", true);
            }
            if (GameManager.GM.curLap == 3)
            {
                anim.SetBool("isTwo", false);
                anim.SetBool("isThree", true);
            }

            lap.GetComponent<Animator>().SetTrigger("Change");
        }

        // ײ�Ž���
        if (collision.tag == "Door")
        {
            if (curKeys == 3)
            {
                GameManager.GM.ShowGameDone();
                SoundManager.SM.PauseBGM();
            }
        }

        // ��������
        if (collision.tag == "FlyArea")
        {
            isFlying = !isFlying; 
            if (isFlying)
            {
                blackBG.SetActive(true); 

                speedBeforeFly = curSpeed;

                sr.enabled = false; 

                switch (GameManager.GM.curLap)
                {
                    case 1:
                        airPlane1.SetActive(true); 
                        if (curDirection < 0)
                        {
                            airPlane1.GetComponent<SpriteRenderer>().flipX = true; 
                        }
                        else
                        {
                            airPlane1.GetComponent<SpriteRenderer>().flipX = false; 
                        }
                        break;
                    case 2:
                        airPlane2.SetActive(true);
                        if (curDirection < 0)
                        {
                            airPlane2.GetComponent<SpriteRenderer>().flipX = true; 
                        }
                        else
                        {
                            airPlane2.GetComponent<SpriteRenderer>().flipX = false; 
                        }
                        break;
                    case 3:
                        airPlane3.SetActive(true);
                        if (curDirection < 0)
                        {
                            airPlane3.GetComponent<SpriteRenderer>().flipX = true; 
                        }
                        else
                        {
                            airPlane3.GetComponent<SpriteRenderer>().flipX = false; 
                        }
                        break; 
                    default:
                        break;
                }
            }
            else
            {
                blackBG.SetActive(false); 

                curSpeed = speedBeforeFly;

                sr.enabled = true; 

                airPlane1.SetActive(false);
                airPlane2.SetActive(false);
                airPlane3.SetActive(false); 
            }

            Portal.Play();
            
        }

    }
    // ���봥����
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Key")
        {
            curKeys++;
            SoundManager.SM.PlayKeyGrab();
            Destroy(collision.gameObject); 
        }

        if (collision.tag == "SlopeEnd")
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f); 
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
