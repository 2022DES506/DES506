using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalScore : MonoBehaviour
{
    private Text totalScore;

    
    // Start is called before the first frame update
    void Start()
    {
        totalScore = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        totalScore.text = PointsSystem.PS.GetCurrentPoints().ToString();
    }
}
