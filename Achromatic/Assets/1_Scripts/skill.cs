using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class skill : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    private Status status;
    HpScript hpScript;
    public VisualEffect VEA;


    public float timer;
    private float time;
    void Start()
    {
        //time = timer;
        hpScript = GameObject.Find("Hpbar").GetComponent<HpScript>();
        player = GameObject.FindWithTag("player");
        status = player.GetComponent<Status>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused == false)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
            }
            else
            {

                if (Input.GetKeyDown(KeyCode.E))
                {
                    VisualEffect visualEffectInstance = Instantiate(VEA, new Vector3(transform.position.x, transform.position.y + 0.1f - 1, transform.position.z), transform.rotation);
                    visualEffectInstance.Play();

                    if (status.hp < status.Maxhp)
                    {
                        time = timer;
                        status.hp += 25;
                        if(status.hp >= status.Maxhp)
                        {
                            status.hp = status.Maxhp;
                        }
                        hpScript.UpdateHp(status.hp);
                    }
                }

            }
        }
    }
}
