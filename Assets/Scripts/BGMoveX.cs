using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMoveX : MonoBehaviour
{
    private void Update()
    {
        BGPicMovement(); 
    }

    private void BGPicMovement()
    {
        if (GameManager.GM.playerDir == 1)
        {
            if ((GameManager.GM.playerPos.x - transform.position.x) >= 30f)
            {
                transform.position = new Vector2(transform.position.x + 60f, transform.position.y); 
            }
        }
        if (GameManager.GM.playerDir == -1)
        {
            if ((transform.position.x - GameManager.GM.playerPos.x) >= 30f)
            {
                transform.position = new Vector2(transform.position.x - 60f, transform.position.y); 
            }
        }
    }
}
