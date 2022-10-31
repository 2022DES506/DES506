using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMoveY : MonoBehaviour
{
    [SerializeField]
    private float playerTop, backgroundTop;

    private void Update()
    {
        CheckPosY(); 
    }

    private void CheckPosY()
    {
        if (!GameManager.GM.layerChangeLock)
        {
            float _newPosY;
            _newPosY = GameManager.GM.playerPos.y + backgroundTop - GameManager.GM.playerPos.y / playerTop * backgroundTop;

            transform.position = new Vector2(transform.position.x, _newPosY);

            GameManager.GM.layerChangeLock = true; 
        }
    }
}
