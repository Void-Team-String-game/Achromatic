using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class Mob : MonoBehaviour
{
    Status playerstatus;
    NavMeshAgent nav;
    public Transform target;

    public float health = 100f;
    public float damage = 5f;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        playerstatus = GameObject.FindWithTag("Player").GetComponent<Status>();
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

    public void Attack()
    {
        playerstatus.hp -= damage;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

