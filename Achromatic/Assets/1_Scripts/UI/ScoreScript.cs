using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    Text scoretext;
    Status playerstatus;

    void Awake()
    {
        scoretext = GameObject.Find("Score_value").GetComponent<Text>();
        playerstatus = GameObject.FindWithTag("Player").GetComponent<Status>();
    }

    public void UpdateScore(float score)
    {
        playerstatus.score += score;
        scoretext.text = playerstatus.score.ToString();
    }
}
