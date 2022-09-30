using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBGMove : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField] 
    private float maxMove;

    private float curMove;

    private void Start()
    {
        curMove = 0; 
    }

    private void Update()
    {
        MoveBG(); 
    }

    private void MoveBG()
    {
        Vector2 newPos = transform.position;
        if (curMove < maxMove)
        {
            newPos.y += moveSpeed * Time.deltaTime;
            curMove += moveSpeed * Time.deltaTime;
        }
        else
        {
            newPos.y -= maxMove; 
            curMove = 0; 
        }
        transform.position = newPos; 
    }

}
