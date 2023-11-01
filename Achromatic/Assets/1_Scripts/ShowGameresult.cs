using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowGameresult : MonoBehaviour
{
    public Text rank;
    public Text currentscore;
    RankMain saveController;
    
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        currentscore.text = UserData.score.ToString();

        if(UserData.score == 0)
        {
            rank.text = "F";
        }
        else if (UserData.score < 1000)
        {
            rank.text = "E";
        }
    }
}
