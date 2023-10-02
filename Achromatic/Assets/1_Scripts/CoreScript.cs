using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreScript : MonoBehaviour
{
    // Start is called before the first frame update
    Slider Corehp;
    public float maxcorehp;
    public float currentcorehp;

    private void Awake()
    {
        Corehp = GameObject.Find("Corebar").GetComponent<Slider>();
        currentcorehp = maxcorehp;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentcorehp <= 0)
        {
            // 게임 종료
        }    
    }

    public void UpdateHp(float hp)
    {
        currentcorehp += hp;
        if(currentcorehp > maxcorehp)
        {
            currentcorehp = maxcorehp;
        }

        Corehp.value = (float)currentcorehp/maxcorehp;
    }
}
