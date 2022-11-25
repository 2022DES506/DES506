using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipClose : MonoBehaviour
{
    private Vector2 startPos;
    private float RespawnTimer;
    private SpriteRenderer sr;
    private bool isFollow;
    private PlayerControl player;
    private Animator anim;

    private void Start()
    {
        startPos = transform.position;
        RespawnTimer = 0f;
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>(); 
        if (player == null)
        {
            player = FindObjectOfType<PlayerControl>();
        }
    }

    private void Update()
    {
        if (isFollow && player != null)
        {
            transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
        }

        if (RespawnTimer > 0f)
        {
            RespawnTimer -= Time.deltaTime; 
            if (RespawnTimer <= 0f)
            {
                sr.enabled = true; 
                RespawnTimer = 0f;
            }
        }
    }

    public void CloseAnimStart()
    {
        isFollow = true;
    }

    public void CloseAnimEnd()
    {
        RespawnTimer = 10f;
        sr.enabled = false;
        isFollow = false;
        transform.position = startPos;

        anim.SetBool("isMove", false);

    }
}
