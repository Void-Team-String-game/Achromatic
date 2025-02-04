using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private GameObject mob;
    public float cooltime;
    private bool summonlock = false;

    private void Start()
    {
        StartCoroutine(SummonMob(cooltime));
    }
    void Update()
    {
        if(summonlock == false)
        {
            Instantiate(mob, gameObject.transform.position, Quaternion.identity);
            StartCoroutine(SummonMob(cooltime));
        }
        
    }

    IEnumerator SummonMob(float timer)
    {
        summonlock = true;
        yield return new WaitForSeconds(timer);
        summonlock = false;
    }
}
