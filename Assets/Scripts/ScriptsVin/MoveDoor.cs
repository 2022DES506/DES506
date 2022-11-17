///////////////////////            Movement of the road blocking wall in front of the key


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoor : MonoBehaviour
{
    [SerializeField]
    private float lapY1, lapY2, lapY3;

    private void Update()
    {
        switch (GameManager.GM.curLap)
        {
            case 1:
                transform.position = new Vector2(transform.position.x, lapY1); 
                break;
            case 2:
                transform.position = new Vector2(transform.position.x, lapY2); 
                break;
            case 3:
                transform.position = new Vector2(transform.position.x, lapY3); 
                break;
            default:
                break;
        }
    }
}
