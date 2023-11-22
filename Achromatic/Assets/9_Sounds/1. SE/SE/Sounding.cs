using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounding : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("Waiting");
    }

    private IEnumerator Waiting()
    {
        yield return new WaitForSeconds(1.0f);

        Destroy(gameObject);
    }
}
