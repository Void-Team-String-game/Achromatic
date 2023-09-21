using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpScript : MonoBehaviour
{
    Slider[] Hpbar = new Slider[4];
    Text Hptext;
    Image BloodShadow;

    // Start is called before the first frame update
    void Awake()
    {
        Hptext = GameObject.Find("Hpvalue1").GetComponent<Text>();
        BloodShadow = GameObject.Find("BloodShadow").GetComponent<Image>();
        for(int i = 0; i < 4; i++)
        {
            string name = "Hpbar" + (i+1).ToString();
            Hpbar[i] = GameObject.Find(name).GetComponent<Slider>();
        }
    }

    // Update is called once per frame
    public void UpdateHp(float z)
    {
        float alpha = 0.99f;
        Hptext.text = ((int)z).ToString();
        for(int i=0; i<4; i++)
        {
            if (z - 25 > 0)
            {
                Hpbar[i].value = 1f;
                alpha -= 0.33f;
            }
            else Hpbar[i].value = (float)z / 25;

            z -= 25;
        }

        Color blood = BloodShadow.color;

        BloodShadow.color = new Color(blood.r, blood.g, blood.b, alpha);
    }
}
