using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeMenu : MonoBehaviour
{
    public void Home()
    {
        GameObject.Find("RankManager").GetComponent<RankMain>().post();
        SceneManager.LoadScene("MainMenu");
    }
}
