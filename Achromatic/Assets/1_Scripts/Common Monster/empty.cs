using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class empty : MonoBehaviour
{
    public Transform Target;
    Rigidbody rb;
    BoxCollider boxCollider;
    Material mat;
    NavMeshAgent nav;

    // Start is called before the first frame update
    void Awake()
    {
      rb=GetComponent<Rigidbody>();
      boxCollider=GetComponent<BoxCollider>();
      mat=GetComponent<MeshRenderer>().material;
      nav=GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(Target.transform.position);
    }

    
}
