using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class Mob : MonoBehaviour
{
    HpScript hpScript;
    CoreScript coreScript;
    ScoreScript scoreScript;
    Status playerstatus;
    NavMeshAgent nav;
    GameObject player;
    Animator anim;
    public Transform target;

    public float health = 100f;
    public float damage = 5f;
    public float score = 100f;
    private bool attacklock = false;
    [HideInInspector]
    public bool coroutinelock = false;

    private void Start()
    {
        hpScript = GameObject.Find("Hpbar").GetComponent<HpScript>();
        coreScript = GameObject.FindWithTag("core").GetComponent<CoreScript>();
        scoreScript = GameObject.Find("Score_value").GetComponent<ScoreScript>();
        nav = GetComponent<NavMeshAgent>();
        playerstatus = GameObject.FindWithTag("Player").GetComponent<Status>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();

        target = GameObject.FindWithTag("core").transform;
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

        if (damage >= playerstatus.hp)
        {
            damage = playerstatus.hp;
        }
        playerstatus.hp -= damage;
        hpScript.UpdateHp(playerstatus.hp);

        yield return new WaitUntil(()=>!anim.GetCurrentAnimatorStateInfo(0).IsName("attack1"));
        attacklock = false;
    }

    private void Die()
    {
        scoreScript.UpdateScore(score);
        Destroy(gameObject);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            scoreScript.UpdateScore(0);
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("attack1"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.9f && attacklock==false)
                {
                    StartCoroutine(Attack());
                }
            }
            else anim.SetTrigger("attack1");
        }
        if (other.gameObject.tag == "core")
        {
            Debug.Log("Core attacked");
            coreScript.UpdateHp(-damage);
            Destroy(gameObject);
        }
    }
}

