using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    private float timeBtwSpawns;
    [SerializeField]
    private float startTimeBtwSpawns;
    [SerializeField]
    private GameObject echoPrefab;
    private PlayerControl player;

    private void Start()
    {
        player = GetComponent<PlayerControl>(); 
    }

    private void Update()
    {
        if (player.isSpeedUping) 
        {
            if (timeBtwSpawns <= 0)
            {
                GameObject _echo = Instantiate(echoPrefab, transform.position, Quaternion.identity);
                Destroy(_echo, 2f);
                timeBtwSpawns = startTimeBtwSpawns;
            }
            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }
    }
}
