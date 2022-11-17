/////////////           Trampoline animation 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimControl : MonoBehaviour
{
    private Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>(); 
    }

    public void SpringEndEvent()
    {
        ani.SetBool("isSpring", false); 
    }
}
