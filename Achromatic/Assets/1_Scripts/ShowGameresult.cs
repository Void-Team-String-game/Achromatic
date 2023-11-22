using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowGameresult : MonoBehaviour
{
    public Text rank;
    public Text currentscore;
    RankMain saveController;

    [SerializeField]
    private SoundManager soundManager;
    
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
        else if (UserData.score < 3000)
        {
            rank.text = "D";
        }
        else if (UserData.score < 5000)
        {
            rank.text = "C";
        }
        else if (UserData.score < 7500)
        {
            rank.text = "B";
        }
        else if (UserData.score < 10000)
        {
            rank.text = "A";
        }
        else if (UserData.score < 15000)
        {
            rank.text = "S";
        }
        else if (UserData.score < 20000)
        {
            rank.text = "SS";
        }
        else if (UserData.score < 30000)
        {
            rank.text = "U";
        }
        else
        {
            rank.text = "X";
        }

        if(UserData.score == 0)
            soundManager.Personal_PlaySound("Failed", false);
        else if(UserData.score < 5000)
            soundManager.Personal_PlaySound("AlmostFailed", false);
        else if (UserData.score < 10000)
            soundManager.Personal_PlaySound("Succeed", false);
        else
            soundManager.Personal_PlaySound("WonderfulSucceed", false);
    }
}
