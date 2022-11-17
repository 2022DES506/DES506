///////////////////////     point system

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsSystem : MonoBehaviour
{
    public static PointsSystem PS; 

    private Text pointsNum;
    private int curPoints;

    [SerializeField]
    private float NGTimer = 1f;
    [SerializeField]
    private float SHTimer = 2f;
    [SerializeField]
    private float GFTimer = 10f;
    [SerializeField]
    private float CFTimer = 5f;

    private float curNGTimer; 
    private float curSHTimer; 
    private float curGFTimer; 
    public float curGFCoins;
    private float curCFTimer;

    private PlayerControl player;

    private void OnEnable()
    {
        if (PS == null)
        {
            PS = this; 
        }
    }

    private void Start()
    {
        pointsNum = GetComponent<Text>();
        player = FindObjectOfType<PlayerControl>(); 
        curPoints = 0;

        curNGTimer = NGTimer;
        curSHTimer = SHTimer;
        curGFTimer = GFTimer;
        curGFCoins = 0;
        curCFTimer = CFTimer; 
    }

    private void Update()
    {
        // NaturalGrowth(); 
        SpeedHold();
        GoldFanatic(); 
        pointsNum.text = curPoints.ToString();
        CollisionFree(); 
    }

    public void AddPoints(int _num)
    {
        curPoints += _num; 
    }

    public void ResetCFTimer()
    {
        curCFTimer = CFTimer; 
    }

    private void CollisionFree()
    {
        curCFTimer -= Time.deltaTime; 
        if (curCFTimer < 0)
        {
            curPoints += 1000; 
            ResetCFTimer();
            Debug.Log("Collision Free!");
            player.ShowScoreMessage("Avoided Collision!");
        }
    }

    private void SpeedHold()
    {
        if (!GameManager.GM.isSpeedUp)
        {
            curSHTimer = SHTimer; 
        }
        curSHTimer -= Time.deltaTime; 
        if (curSHTimer < 0)
        {
            curPoints += 500; 
            curSHTimer = SHTimer;
            Debug.Log("Speed Hold!");
            player.ShowScoreMessage("Kept a Constant Speed!");
        }
    }

    private void NaturalGrowth()
    {
        if (GameManager.GM.isSlowDown) 
        {
            curNGTimer = NGTimer; 
        }
        curNGTimer -= Time.deltaTime; 
        if (curNGTimer < 0)
        {
            curPoints += 100; 
            curNGTimer = NGTimer; 
        }
    }

    private void GoldFanatic()
    {
        curGFTimer -= Time.deltaTime; 
        if (curGFTimer < 0)
        {
            curGFCoins = 0; 
            curGFTimer = GFTimer; 
        }
        else
        {
            if (curGFCoins >= 20)
            {
                curPoints += 500;
                curGFCoins = 0;
                curGFTimer = GFTimer;
                Debug.Log("Gold Fanatic!");
                player.ShowScoreMessage("Coin Collector!");
            }
        }
    }
}
