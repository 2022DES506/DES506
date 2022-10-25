using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 声明单例
    public static GameManager GM;

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
    }

    public void ShowGameDone()
    {
        GameDoneUI.SetActive(true); 
    }

}
