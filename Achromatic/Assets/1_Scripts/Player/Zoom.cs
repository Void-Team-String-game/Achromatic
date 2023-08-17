using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField]
    public Camera Cam;
    public GameObject crosshair;
    public GameObject player;
    public GameObject gun;
    public bool zoommode;
    // private float tempRunspeed;
    // private float tempWalkspeed;

    Gun gunscript;
    Status status;
    Animator Camanim;
    Animator Gunanim;
    // Update is called once per frame

    void Start()
    {
        gunscript = gun.GetComponent<Gun>();
        status = player.GetComponent<Status>();
        Camanim = Cam.GetComponent<Animator>();
        Gunanim = gun.GetComponent<Animator>();
       // tempRunspeed = status.runSpeed;
       // tempWalkspeed = status.walkSpeed;
    }

    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(1))
        { 
            zoommode = !zoommode;
            changeaim();
        }
        */

        if (!gunscript.reloading)
        {
            if (Input.GetMouseButton(1))
            {
                zoommode = true;
                changeaim();
            }
            else
            {
                zoommode = false;
                changeaim();
            }
        }
        else
        {
            zoommode = false;
            changeaim();
        }


        /*
            if (Input.GetMouseButton(1))
            {
                // transform.position = zoompos.transform.position;
                transform.position = Vector3.Lerp(transform.position, zoompos.transform.position, 0.05f);
                 crosshair.SetActive(false);
            }
            else
            {
               // transform.position = zoomoffpos.transform.position;
               transform.position = Vector3.Lerp(transform.position, zoomoffpos.transform.position, 0.05f);
                crosshair.SetActive(true);
            }
        */

    }

    void changeaim()
    {
        if (zoommode == true)
        {
            // status.runSpeed = tempRunspeed / 3;
            // status.walkSpeed = tempWalkspeed / 3;
            crosshair.SetActive(false);
            Camanim.SetBool("Zooming", true);
            Gunanim.SetBool("Zooming", true);
        }
        else
        {
            // status.runSpeed = tempRunspeed;
            // status.walkSpeed = tempWalkspeed;
            if(Time.timeScale == 0)
            {
                crosshair.SetActive(false);
            }
            else
            {
                crosshair.SetActive(true);
            }
            Camanim.SetBool("Zooming", false);
            Gunanim.SetBool("Zooming", false);
        }
    }
}
