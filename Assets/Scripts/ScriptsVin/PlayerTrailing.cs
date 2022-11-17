/////////////////       Player Trailing Controls


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrailing : MonoBehaviour
{
    private TrailRenderer tr;
    [SerializeField]
    private PlayerControl player; 

    private void Start()
    {
        tr = GetComponent<TrailRenderer>(); 
    }

    private void Update()
    {
        if (player.isSpeedUping)
        {
            tr.enabled = false; 
        }
        else
        {
            tr.enabled = true; 
            switch (GameManager.GM.curLap)
            {
                case 1:
                    tr.startColor = Color.green;
                    break;
                case 2:
                    tr.startColor = new Color(1f, 0.38f, 0f, 1f);
                    break;
                case 3:
                    tr.startColor = Color.red;
                    break;
                default:
                    break;
            }
        }
    }
}
