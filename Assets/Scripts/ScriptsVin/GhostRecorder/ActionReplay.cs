using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionReplay : MonoBehaviour
{
    [SerializeField]
    private List<GhostShot> actionReplayRecords = new List<GhostShot>();

    [SerializeField]
    private bool isInReplayMode;
    [SerializeField]
    private float currentReplayIndex;
    [SerializeField]
    private float indexChangeRate; 

    private SpriteRenderer sr;
    private Rigidbody2D rb; 

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>(); 
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y+Time.deltaTime); 
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector2(transform.position.x-Time.deltaTime, transform.position.y);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y-Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector2(transform.position.x+Time.deltaTime, transform.position.y);
        }

        if (Input.GetKeyDown(KeyCode.R))  
        {
            isInReplayMode = !isInReplayMode; 

            if (isInReplayMode)
            {
                SetTransform(0);
                rb.isKinematic = true;
                rb.velocity = Vector2.zero; 
            }
            else
            {
                SetTransform(actionReplayRecords.Count - 1);
                rb.isKinematic = false; 
            }
        }

        indexChangeRate = 0f;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            indexChangeRate = 1f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            indexChangeRate = -1f;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            indexChangeRate *= 0.5f;
        }
    }

    private void FixedUpdate()
    {
        if (isInReplayMode == false)
        {
            actionReplayRecords.Add(new GhostShot { posMark = transform.position, dirMark = sr.flipX });
        }
        else
        {
            float nextIndex = currentReplayIndex + indexChangeRate;

            if (nextIndex < actionReplayRecords.Count && nextIndex >= 0)
            {
                SetTransform(nextIndex);
            }
        }
    }

    private void SetTransform(float index)
    {
        currentReplayIndex = index; 

        GhostShot actionReplayRecord = actionReplayRecords[(int)index]; 

        transform.position = actionReplayRecord.posMark;
        sr.flipX = actionReplayRecord.dirMark; 
    }
}
