using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{

    private Text Timer;

    void Start()
    {
        Timer = GetComponent<Text>(); 
    }

    void Update()
    {
        TimeFlies(); 
        Timer.text = CalculateTime(); 
    }

    void TimeFlies()
    {
        GameManager.GM.curTimer -= Time.deltaTime; 
    }

    string CalculateTime()
    {
        string CDtime = "";
        int timeValue = (int)(GameManager.GM.curTimer * 100);
        int tenTimesMilliseconds = timeValue % 100;
        timeValue = timeValue / 100;
        int seconds = timeValue % 60;
        int minutes = timeValue / 60;
        CDtime = (minutes / 10).ToString() + (minutes % 10).ToString() + ":"
            + (seconds / 10).ToString() + (seconds % 10).ToString() + ":"
            + (tenTimesMilliseconds / 10).ToString() + (tenTimesMilliseconds % 10).ToString(); 

        return CDtime;
    }
}
