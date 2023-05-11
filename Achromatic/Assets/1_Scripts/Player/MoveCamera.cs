using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform CamPosition;

    // Update is called once per frame
    // 업데이트
    void Update()
    {
        transform.position = CamPosition.position;
    }
}
