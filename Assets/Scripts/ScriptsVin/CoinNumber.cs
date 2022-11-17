//////////////////////         Dynamic display of the number of coins collected
/////////            Feature temporarily deprecated


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinNumber : MonoBehaviour
{
    private Text coinNumber;
    public Animator animator;

    private void Start()
    {
        coinNumber = GetComponent<Text>(); 
    }

    private void Update()
    {
        coinNumber.text = GameManager.GM.curCoins.ToString();
    }
}
