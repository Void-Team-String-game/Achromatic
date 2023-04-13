using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float Sens;

    public Transform orientation;

    private float xRotation;
    private float yRotation;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 중앙좌표에 고정
        Cursor.visible = false; // 마우스 커서 안보이게 하기
    }

    private void Update()
    {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * Sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * Sens;

        yRotation += mouseX; // 왼쪽 오른쪽이므로 mouseX 더해주기

        xRotation -= mouseY; // 위 아래 이므로 mouseY 빼주기
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 위 아래로 한바퀴 안돌도록 처리

        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); // 캠에 적용
        orientation.rotation = Quaternion.Euler(0, yRotation, 0); // 보는 방향 저장
    }
}
