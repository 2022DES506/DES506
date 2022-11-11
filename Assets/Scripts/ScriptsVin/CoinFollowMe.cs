using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFollowMe : MonoBehaviour
{

    [SerializeField]
    private float speed = 25f;
    [SerializeField]
    private float height = 3f; 
    [SerializeField]
    private float timer = 0.15f; 
    [SerializeField]
    private float minDist = 0.1f;

    private Vector2 endPos; 

    private void Start()
    {
        endPos = new Vector2(transform.position.x, transform.position.y + height); 
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, endPos, speed * Time.deltaTime);  
        }
        else
        {
            if (((Vector2)transform.position - GameManager.GM.playerPos).magnitude > minDist)
            {

                transform.position = Vector2.MoveTowards(transform.position, GameManager.GM.playerPos, speed * Time.deltaTime);
            }
            else
            {
                Destroy(gameObject); 
            }
        }
    }
}
