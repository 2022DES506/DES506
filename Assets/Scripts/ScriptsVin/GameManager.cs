using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 声明单例
    public static GameManager GM;

    public enum LayerState
    {
        layerDown,
        layerRight1,
        layerRight2,
        layerRight3,
        layerRight4,
        layerUp,
        layerLeft4, 
        layerLeft3,
        layerLeft2, 
        layerLeft1,  
    };

    // 可调控变量
    [SerializeField]
    private int defaultTimer;

    // 当前状态变量
    public int curLap; 
    public int curLevel;
    public int curCoins;
    public float curTimer;

    [SerializeField]
    private GameObject GameDoneUI;

    [SerializeField]
    private GameObject CD3, CD2, CD1, CDGo;

    [SerializeField]
    private GameObject CoinsLap1, CoinsLap2, CoinsLap3;
    [SerializeField]
    private GameObject BGLap1, BGLap2, BGLap3;
    [SerializeField]
    private GameObject CamGR, CamGRb, CamGL, CamGLb; 

    [SerializeField]
    private GameObject Background;
    private bool changeBG;

    public Vector2 playerPos; // 角色实时位置
    public int playerDir;  // 角色实时方向
    public bool isSpeedUp;
    public bool isSlowDown; 

    public bool layerChangeLock;

    public LayerState curLayerState;


    private void OnEnable()
    {
        // 初始化单例
        if (GM == null)
        {
            GM = this; 
        }
    }

    private void Start()
    {
        curLap = 0; 
        curLevel = 0;
        curCoins = 0;
        curTimer = defaultTimer * 60; // 浮点数秒来计算时间

        ShowCountdown();

        changeBG = false;
        layerChangeLock = false;

        curLayerState = LayerState.layerLeft1; 

    }

    private void Update()
    {
        BackGroundCheck();
        ItemsLapShowHide();
        CameraCheck(); 
    }

    private void CameraCheck()
    {
        switch (playerDir)
        {
            // 加速
            case 1:
                if (isSpeedUp)
                {
                    CamGR.SetActive(false); 
                    CamGRb.SetActive(true);
                }
                else
                {
                    CamGR.SetActive(true);
                    CamGRb.SetActive(false); 
                }
                CamGL.SetActive(false);
                CamGLb.SetActive(false); 
                break;
            // 减速
            case -1:
                if (isSpeedUp)
                {
                    CamGL.SetActive(false);
                    CamGLb.SetActive(true);
                }
                else
                {
                    CamGL.SetActive(true); 
                    CamGLb.SetActive(false);
                }
                CamGR.SetActive(false);
                CamGRb.SetActive(false); 
                break;
            default:
                break;
        }
    }

    private void ItemsLapShowHide()
    {
        switch (curLap)
        {
            case 1:
                CoinsLap1.SetActive(true);
                CoinsLap2.SetActive(false);
                CoinsLap3.SetActive(false);
                BGLap1.SetActive(true);
                BGLap2.SetActive(false);
                BGLap3.SetActive(false);
                break;
            case 2:
                CoinsLap1.SetActive(false);
                CoinsLap2.SetActive(true);
                CoinsLap3.SetActive(false);
                BGLap1.SetActive(false);
                BGLap2.SetActive(true);
                BGLap3.SetActive(false);
                break;
            case 3:
                CoinsLap1.SetActive(false);
                CoinsLap2.SetActive(false);
                CoinsLap3.SetActive(true);
                BGLap1.SetActive(false);
                BGLap2.SetActive(false);
                BGLap3.SetActive(true); 
                break; 
            default:
                break;
        }
    }

    private void BackGroundCheck()
    {
        if (changeBG == false && curLap == 2)
        {
            
            changeBG = !changeBG; 
        }
        if (changeBG == true && curLap == 3)
        {

        }
    }

    #region Countdown 开场倒计时

    private void ShowCountdown()
    {
        OpenCD3();
        Invoke("CountdownGoTwo", 1f); 
    }
    private void CountdownGoTwo()
    {
        CloseCD3();
        OpenCD2();
        Invoke("CountdownGoOne", 1f); 
    }
    private void CountdownGoOne()
    {
        CloseCD2();
        OpenCD1();
        Invoke("CountdownGoGo", 1f);
    }
    private void CountdownGoGo()
    {
        CloseCD1();
        OpenCDGo();
        Invoke("CloseCDGo", 1f); 
    }
    private void OpenCD1()
    {
        CD1.SetActive(true); 
    }
    private void OpenCD2()
    {
        CD2.SetActive(true); 
    }
    private void OpenCD3()
    {
        CD3.SetActive(true); 
    }
    private void OpenCDGo()
    {
        CDGo.SetActive(true); 
    }
    private void CloseCD1()
    {
        CD1.SetActive(false); 
    }
    private void CloseCD2()
    {
        CD2.SetActive(false); 
    }
    private void CloseCD3()
    {
        CD3.SetActive(false); 
    }
    private void CloseCDGo()
    {
        CDGo.SetActive(false);

        SoundManager.SM.PlayBGM(); 
    }

    #endregion


    public void ShowGameDone()
    {
        PointsSystem.PS.AddNewRecord(); 
        GameDoneUI.SetActive(true); 
    }



}
