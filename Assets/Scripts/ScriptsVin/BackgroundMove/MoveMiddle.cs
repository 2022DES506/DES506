using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBack : MonoBehaviour
{
    [SerializeField]
    private float moveXFineTuning = 10;

    private void Update()
    {
        PosCheck(); 
    }

    private void PosCheck()
    {
        if (GameManager.GM.playerDir == 1)
        {
            transform.position = new Vector2(transform.position.x + moveXFineTuning * Time.deltaTime, transform.position.y);  // Fine-tuning

            // Check
            if ((GameManager.GM.playerPos.x - transform.position.x) > 30f)
            {
                transform.position = new Vector2(transform.position.x + 60f, transform.position.y);
            }
            if ((transform.position.x - GameManager.GM.playerPos.x) > 30f)
            {
                transform.position = new Vector2(transform.position.x - 60f, transform.position.y);
            }
        }
        if (GameManager.GM.playerDir == -1)
        {
            transform.position = new Vector2(transform.position.x - moveXFineTuning * Time.deltaTime, transform.position.y);  // Fine-tuning 

            // Check
            if ((GameManager.GM.playerPos.x - transform.position.x) > 30f)
            {
                transform.position = new Vector2(transform.position.x + 60f, transform.position.y);
            }
            if ((transform.position.x - GameManager.GM.playerPos.x) > 30f)
            {
                transform.position = new Vector2(transform.position.x - 60f, transform.position.y);
            }
        }
    }
}
