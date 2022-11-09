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
        lapNumber.text = GameManager.GM.curLap.ToString();
    }
}
