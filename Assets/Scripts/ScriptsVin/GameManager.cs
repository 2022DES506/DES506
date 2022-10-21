using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ��������
    public static GameManager GM;

    // �ɵ��ر���
    [SerializeField]
    private int defaultTimer;

    // ��ǰ״̬����
    public int curLap; 
    public int curLevel;
    public int curCoins;
    public float curTimer;

    public bool canStartRecording, canStopRecording, canStartReplay, canStopReplay; 

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

        canStartRecording = true;
        canStopRecording = false;
        canStartReplay = false;
        canStopReplay = false; 
    }

}
