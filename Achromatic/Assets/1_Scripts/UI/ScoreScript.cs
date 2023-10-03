using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    Text scoretext;

    void Awake()
    {
        scoretext = GameObject.Find("Score_value").GetComponent<Text>();
    }

    public void UpdateScore(float score)
    {
        UserData.score += score;
        scoretext.text = UserData.score.ToString();
    }
}
