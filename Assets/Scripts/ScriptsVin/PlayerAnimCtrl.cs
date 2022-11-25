/////////////////////////////////                Transition logic for jump animation
////////////////////         Temporarily deprecated, needs to be rewritten


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimCtrl : MonoBehaviour
{
    private PlayerControl player; 
    private Animator anim;
    private bool isGroundBef; 

    private void Start()
    {
        player = GetComponent<PlayerControl>(); 
        anim = GetComponent<Animator>();
        isGroundBef = player.isGround; 
    }

    private void Update()
    {
        JumpCheck(); 
    }

    private void JumpCheck()
    {
        if (player.isGround != isGroundBef)
        {
            SetJumpTrue();
            isGroundBef = player.isGround; 
        }
    }

    public void SetJumpTrue()
    {
        anim.SetBool("isJumpp", true); 
    }

    public void SetJumpFalse()
    {
        anim.SetBool("isJumpp", false); 
    }
}
