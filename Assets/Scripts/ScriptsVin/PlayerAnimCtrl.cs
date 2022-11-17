/////////////////////////////////                Transition logic for jump animation
////////////////////         Temporarily deprecated, needs to be rewritten


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimCtrl : MonoBehaviour
{
    private PlayerControl player; 
    private Animator anim;

    private void Start()
    {
        player = GetComponent<PlayerControl>(); 
        anim = GetComponent<Animator>(); 
    }

    private void Update()
    {
        CheckJumping();
        CheckGround(); 
    }

    public void End2Idle()
    {
        anim.SetBool("isIdle", true); 
        anim.SetBool("isDown", false);
    }

    private void CheckGround()
    {
        if (!anim.GetBool("isJumping") && !anim.GetBool("isDown") && !anim.GetBool("isIdle") && player.isGround)
        {
            anim.SetBool("isDown", true); 
        }
    }

    private void CheckJumping()
    {
        if (!anim.GetBool("isJumping") && player.isJumping)
        {
            anim.SetBool("isJumping", true);
            anim.SetBool("isIdle", false); 
        }
        if (anim.GetBool("isJumping") && !player.isJumping)
        {
            anim.SetBool("isJumping", false); 
        }
    }
}
