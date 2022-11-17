///////////////////////          Echo effect produced by the character when accelerating

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    private float timeBtwSpawns;
    [SerializeField]
    private float startTimeBtwSpawns;
    [SerializeField]
    private GameObject echoPrefab1, echoPrefab2, echoPrefab3; 
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
                GameObject _echo;
                switch (GameManager.GM.curLap)
                {
                    case 1:
                        _echo = Instantiate(echoPrefab1, transform.position, Quaternion.identity);
                        if (GameManager.GM.playerDir == -1)
                        {
                            _echo.GetComponent<SpriteRenderer>().flipX = true; 
                        }
                        Destroy(_echo, 2f);
                        break;
                    case 2:
                        _echo = Instantiate(echoPrefab2, transform.position, Quaternion.identity);
                        if (GameManager.GM.playerDir == -1)
                        {
                            _echo.GetComponent<SpriteRenderer>().flipX = true;
                        }
                        Destroy(_echo, 2f);
                        break;
                    case 3:
                        _echo = Instantiate(echoPrefab3, transform.position, Quaternion.identity);
                        if (GameManager.GM.playerDir == -1)
                        {
                            _echo.GetComponent<SpriteRenderer>().flipX = true;
                        }
                        Destroy(_echo, 2f);
                        break;
                    default:
                        break; 
                }
                
                timeBtwSpawns = startTimeBtwSpawns;
            }
            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }
    }
}
