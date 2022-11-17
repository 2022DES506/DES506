//////////////////////       Dynamic display of the current lap


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapNumber : MonoBehaviour
{
    private Text lapNumber;
    public Animator animator;

    private void Start()
    {
        lapNumber = GetComponent<Text>(); 
    }

    private void Update()
    {
        if (GameManager.GM.curLap != 4)
        {
            lapNumber.text = GameManager.GM.curLap.ToString(); 
        }
    }
}
