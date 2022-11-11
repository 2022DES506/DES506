using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownProp : MonoBehaviour
{
    [SerializeField]
    private GameObject dropCoinPrefab;
    [SerializeField]
    private int maxNum, minNum;
    [SerializeField]
    private float maxY, maxX, minY, minX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DropCoinSpawn();
        }
    }

    private void DropCoinSpawn()
    {
        int _n = Random.Range(minNum, maxNum);
        for (int i = 0; i < _n; ++i)
        {
            GameObject _coin = Instantiate(dropCoinPrefab, transform.position, Quaternion.identity);
            Vector2 _v = Vector2.zero;
            float _xx = Random.Range(minX, maxX);
            float _yy = Random.Range(minY, maxY);
            if (GameManager.GM.playerDir == 1)
            {
                _v = new Vector2(-_xx, _yy);
            }
            else if (GameManager.GM.playerDir == -1)
            {
                _v = new Vector2(_xx, _yy);
            }
            _coin.GetComponent<Rigidbody2D>().velocity = _v;
        }
    }
}
