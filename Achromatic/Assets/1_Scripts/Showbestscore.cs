using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Showbestscore : MonoBehaviour
{
    Text bestscoretext;

    private void Awake()
    {
        bestscoretext = GetComponent<Text>();
    }
    private void Update()
    {
        bestscoretext.text = UserData.bestscore.ToString();
    }
}
