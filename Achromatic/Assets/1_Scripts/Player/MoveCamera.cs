using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform CamPosition;

    // Update is called once per frame
    void Update()
    {
        transform.position = CamPosition.position;
    }
}
