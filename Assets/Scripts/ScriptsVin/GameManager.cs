using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ��������
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

    // �ɵ��ر���
    [SerializeField]
    private int defaultTimer;

    // ��ǰ״̬����
    public int curLap; 
    public int curLevel;
    public int curCoins;
    public float curTimer;

    [SerializeField]
    private GameObject GameDoneUI;

    [SerializeField]
    private GameObject CD3, CD2, CD1, CDGo;

    [SerializeField]
    private GameObject Background;
    private bool changeBG;

    public Vector2 playerPos; // ��ɫʵʱλ��
    public int playerDir;  // ��ɫʵʱ����

    public bool layerChangeLock;

    public LayerState curLayerState;  

    private void OnEnable()
    {
        // ��ʼ������
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
        curTimer = defaultTimer * 60; // ��������������ʱ��

        ShowCountdown();

        changeBG = false;
        layerChangeLock = false;

        curLayerState = LayerState.layerLeft1; 

    }

    private void Update()
    {
        BackGroundCheck();
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

    #region Countdown ��������ʱ

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
    }

    #endregion


    public void ShowGameDone()
    {
        GameDoneUI.SetActive(true); 
    }

}
