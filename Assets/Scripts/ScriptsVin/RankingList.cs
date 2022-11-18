using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingList : MonoBehaviour
{
    [SerializeField] 
    private GameObject rankingListNumPrefab;

    private void Start()
    {
        CreateRankingList(); 
    }

    private void CreateRankingList()
    {
        int[] _rankingList = PlayerPrefsX.GetIntArray("ranking"); 
        for (int i=0; i<5; ++i)
        {
            GameObject _rankNum = Instantiate(rankingListNumPrefab, transform.position, Quaternion.identity, this.transform);
            if (_rankingList.Length > i)
            {
                _rankNum.GetComponent<Text>().text = _rankingList[i].ToString(); 
            }
            if (i == PlayerPrefs.GetInt("highlight"))
            {
                _rankNum.GetComponent<Text>().color = Color.yellow; 
            }
        }
    }
}
