using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private float dashSpeed;

    private bool isDashRight;

    private Rigidbody2D rb;
    private SpriteRenderer sr; 

    private void Start()
    {
        isDashRight = true;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>(); 
    }

    private void Update()
    {
        BasicDash(); 
    }

    private void BasicDash()
    {
        if (isDashRight)
        {
            rb.velocity = new Vector3(dashSpeed, 0, 0);
            sr.flipX = false; 
        }
        else
        {
            rb.velocity = new Vector3(-dashSpeed, 0, 0);
            sr.flipX = true; 
        }
    }

}
