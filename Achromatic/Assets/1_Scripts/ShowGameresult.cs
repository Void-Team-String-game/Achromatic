using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowGameresult : MonoBehaviour
{

    public Text rank;
    public Text currentscore;
    public Text bestscore;
    public GameObject newrecord;
    SaveController saveController;
    
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        saveController = GameObject.Find("SaveManager").GetComponent<SaveController>();

        if(UserData.bestscore < UserData.score)
        {
            UserData.bestscore = UserData.score;
            newrecord.SetActive(true);
        }
        else
        {
            newrecord.SetActive(false);
        }

        currentscore.text = UserData.score.ToString();
        bestscore.text = UserData.bestscore.ToString();

        if(UserData.score == 0)
        {
            rank.text = "F";
        }
        else if (UserData.score < 1000)
        {
            rank.text = "E";
        }

        saveController.Save();
    }
}
