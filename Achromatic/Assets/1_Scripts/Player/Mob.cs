using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class Mob : MonoBehaviour
{
    HpScript hpScript;
    Status playerstatus;
    NavMeshAgent nav;
    GameObject player;
    Animator anim;
    public Transform target;

    public float health = 100f;
    public float damage = 5f;
    public float score = 100f;
    private bool attacklock = false;

    private void Start()
    {
        hpScript = GameObject.Find("Hpbar").GetComponent<HpScript>();
        nav = GetComponent<NavMeshAgent>();
        playerstatus = GameObject.FindWithTag("Player").GetComponent<Status>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        nav.SetDestination(target.transform.position);
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Die();
        }
    }


    IEnumerator Attack()
    {
        attacklock = true;
        playerstatus.hp -= damage;
        hpScript.UpdateHp(playerstatus.hp);

        yield return new WaitUntil(()=>!anim.GetCurrentAnimatorStateInfo(0).IsName("attack1"));
        attacklock = false;
    }

    private void Die()
    {
        playerstatus.score += score;
        Destroy(gameObject);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("attack1"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f && attacklock==false)
                {
                    StartCoroutine(Attack());
                }
            }
            else anim.SetTrigger("attack1");
        }
    }
}

