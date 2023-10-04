using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmoScript : MonoBehaviour
{
    Slider Armobar;
    Text remainarmo;
    Text maxarmo;
    Gun gun;

    // Start is called before the first frame update
    void Awake()
    {
        remainarmo = GameObject.Find("Armo_remain").GetComponent<Text>();
        maxarmo = GameObject.Find("Armo_max").GetComponent<Text>();
        gun = GameObject.FindWithTag("Gun").GetComponent<Gun>();
        Armobar = GameObject.Find("ArmoBar").GetComponent<Slider>();
    }

    // Update is called once per frame
    public void Update()
    {
        maxarmo.text = "/" + gun.weaponsetting.maxarmo.ToString();
        remainarmo.text = gun.armo.ToString();
        Armobar.value = (float)gun.armo/gun.weaponsetting.maxarmo;
    }
}
