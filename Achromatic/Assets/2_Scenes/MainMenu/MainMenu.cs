using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject guide;
    public Animator anim;
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            guide.SetActive(false);
            anim.SetTrigger("Normal");
        }
    }

}
