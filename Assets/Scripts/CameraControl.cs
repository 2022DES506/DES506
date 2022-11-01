using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; 

public class CameraControl : MonoBehaviour
{
    private CinemachineVirtualCamera Cam;

    private void Start()
    {
        Cam = GetComponent<CinemachineVirtualCamera>(); 
    }

    private void Update()
    {
        ScreenCenterChange(); 
    }

    private void ScreenCenterChange()
    {
        switch (GameManager.GM.playerDir)
        {
            case 1:
                break;
            case 0:
                break;
            case -1:
                break; 
            default:
                break;
        }
    }
}
