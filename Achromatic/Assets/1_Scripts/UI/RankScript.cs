using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankScript : MonoBehaviour
{
    IEnumerator SetRank()
    {
        yield return GameObject.Find("RankManager").GetComponent<RankMain>().RankTop3();
    }
    public void showRank()
    {
        StartCoroutine(SetRank());
    }
}