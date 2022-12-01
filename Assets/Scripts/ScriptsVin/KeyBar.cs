using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class KeyBar : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        SetValue(0f); 
    }

    private void Update()
    {
        if (slider.value < GameManager.GM.curLap)
        {
            float _v = slider.value + Time.deltaTime * 1f; 
            SetValue(_v); 
        }
    }

    public void SetValue(float _value)
    {
        slider.value = _value; 
    }

}
