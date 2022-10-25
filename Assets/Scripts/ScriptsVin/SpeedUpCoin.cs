using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpCoin : MonoBehaviour
{
    [SerializeField]
    private float defTimer;
    private SpriteRenderer sr; 

    private float curTimer;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>(); 
        curTimer = 0f; 
    }

    private void Update()
    {
        if (curTimer > 0)
        {
            sr.enabled = false; 

            curTimer -= Time.deltaTime; 
            if (curTimer <= 0)
            {
                curTimer = 0;
                sr.enabled = true; 
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            curTimer = defTimer; 
        }
    }
}
