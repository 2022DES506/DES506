using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMask : MonoBehaviour
{
    [SerializeField]
    private GameObject M2R, M2L;

    private void Update()
    {
        if (M2R && GameManager.GM.playerDir == -1)
        {
            M2R.SetActive(false);
            M2L.SetActive(true);
        }
        else if (M2L) 
        {
            M2R.SetActive(true); 
            M2L.SetActive(false);
        }
    }
}
