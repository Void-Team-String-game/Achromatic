using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Status status;
    CoreScript core;

    public GameObject[] portals;

    Animator anim;
    bool coroutinelock = false;

    bool hardlock = false;

    private void Awake()
    {
        status = GameObject.FindWithTag("player").GetComponent<Status>();
        core = GameObject.FindWithTag("core").GetComponent<CoreScript>();
        anim = GameObject.Find("Shadow").GetComponent<Animator>();
        UserData.score = 0f;
    }

    private void Update()
    {
        if (core.currentcorehp <= 0 && coroutinelock==false) SceneManager.LoadScene("CoreDestroy");
        if (status.hp <= 0)
        {
            StartCoroutine("Timer");
        }

        if(hardlock == false)
        {
            StartCoroutine(Hardgame());
        }
    }

    IEnumerator Timer()
    {
        coroutinelock = true;
        anim.SetBool("Die", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("Die", false);
        coroutinelock = false;
        SceneManager.LoadScene("GameOver");
    }

    IEnumerator Hardgame()
    {
        hardlock = true;
        yield return new WaitForSeconds(10f);
        for(int i=0; i<portals.Length; i++)
        {
            Portal portalscript = portals[i].GetComponent<Portal>();
            if (portalscript.cooltime > 0.5f)
            {
                portalscript.cooltime = (float)portalscript.cooltime * 99/100;
            }
        }
        hardlock = false;
    }
}
