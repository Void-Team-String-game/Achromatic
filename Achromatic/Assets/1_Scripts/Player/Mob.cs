using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class Mob : MonoBehaviour
{
    NavMeshAgent nav;
    public Transform target;

    public float health = 100f;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
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

    private void Die()
    {
        Destroy(gameObject);
    }
}

