using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class Mob : MonoBehaviour
{
    Status playerstatus;
    NavMeshAgent nav;
    GameObject player;
    Animator anim;
    public Transform target;

    public float health = 100f;
    public float damage = 5f;
    public float score = 100f;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        playerstatus = GameObject.FindWithTag("Player").GetComponent<Status>();
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        nav.SetDestination(target.transform.position);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("CurrentStateName")) // 애니메이션 이름이 실행중일 때 실행
        {
            Attack();
        }
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Die();
        }
    }

    public void Attack()
    {
        playerstatus.hp -= damage;
    }

    private void Die()
    {
        playerstatus.score += score;
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            // 공격 모션 실행
        }
    }
}

