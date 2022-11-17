/////////////////////////        Randomly generated circles for background



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRing : MonoBehaviour
{
    [SerializeField]
    private float startSize, endSize; 
    [SerializeField]
    private float changeAngle; 
    [SerializeField]
    private float changeRate; 
    [SerializeField]
    private float changeLifeTime; 

    private float timer; 
    private float zAngle;
    private Vector3 startScale; 

    private void Start()
    {
        startSize = Random.value * startSize;
        endSize = startSize + Random.value * endSize; 
        changeAngle = (Random.value - 0.5f) * changeAngle; 
        changeRate = Random.value * changeRate + 1; 
        changeLifeTime = Random.value * changeLifeTime + 2;

        startScale = transform.localScale * startSize;
        transform.localScale = startScale; 

        zAngle = 0f; 
        timer = changeLifeTime; 
    }

    private void Update()
    {
        timer -= Time.deltaTime; 
        if (timer <= 0)
        {
            Destroy(gameObject); 
        }
        if (transform.localScale.x / startScale.x < endSize)
        {
            transform.localScale = transform.localScale * changeRate;
        }

        // zAngle += changeAngle * Time.deltaTime;
        zAngle = changeAngle; 
        transform.Rotate(0, 0, zAngle, Space.Self); 
    }
}
