using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    public int curLevel; 

    private void OnEnable()
    {
        if (GM == null)
        {
            GM = this; 
        }
    }

    private void Start()
    {
        curLevel = 0; 
    }

}
