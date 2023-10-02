using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Status status;
    CoreScript core;

    Animator anim;

    private void Awake()
    {
        status = GameObject.FindWithTag("Player").GetComponent<Status>();
        core = GameObject.FindWithTag("core").GetComponent<CoreScript>();
        anim = GameObject.Find("Shadow").GetComponent<Animator>();
    }

    private void Update()
    {
        if (core.currentcorehp <= 0) SceneManager.LoadScene("CoreDestroy");
        if (status.hp <= 0)
        {
            StartCoroutine("Timer");
        }
    }

    IEnumerator Timer()
    {
        anim.SetBool("Die", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("Die", false);
        SceneManager.LoadScene("GameOver");
    }
}
