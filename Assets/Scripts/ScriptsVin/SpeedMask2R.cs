///////////////      Animation logic for the air hood when the character runs to the right


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMask2R : MonoBehaviour
{
    private Animator anim;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.GM.isSpeedUp && !anim.GetBool("isUping"))
        {
            anim.SetBool("isUping", true);
        }
        else if (!GameManager.GM.isSpeedUp && anim.GetBool("isUping"))
        {
            anim.SetBool("isUping", false);
            anim.SetBool("isStartEnd", false);
        }

    }

    public void SpeedUpStartEnd()
    {
        anim.SetBool("isStartEnd", true);
    }
}
