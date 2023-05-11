using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;                   // 방향
    public Transform playerObj;                     // PlayerObj
    private Rigidbody rb;                           // RigidBody
    private PlayerMovement pm;                      // PlayerMovement 스크립트

    [Header("Sliding")]
    public float maxSlideTime;                      // 최대 슬라이딩 시간
    public float slideForce;                        // 슬라이딩 힘
    private float slideTimer;                       // 시간 타이머

    public float slideYScale;                       // Sliding시 YScale
    private float startYScale;                      // 시작시 YScale

    [Header("Input")]
    public KeyCode Slidekey = KeyCode.LeftControl;  // 슬라이딩 키
    private float horizontalInput;                  
    private float verticalInput;                    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        startYScale = playerObj.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(Slidekey) && (horizontalInput != 0 || verticalInput != 0)) // 안움직이는 상태에서 슬라이딩 못하게 막기
            StartSlide();

        if (Input.GetKeyUp(Slidekey) && pm.sliding)
            StopSlide();
    }

    private void StartSlide()
    {
        pm.sliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z); // 플레이어 높이 변경
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse); // 남는 공간 땜시 아래로 밀어주기

        slideTimer = maxSlideTime; // Timer를 maxTime으로 조정
    }

    private void FixedUpdate()
    {
        if (pm.sliding) // 슬라이딩시
            SlidingMovement();
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; // 보고 있는 방향의 앞에 VerticalInput(W, S)값 곱하고 방향 오르쪽에 HorizontalInput(A, D)에 곱해주기

        //sliding normal
        if (!pm.OnSlope() || rb.velocity.y > -0.1f) // 경사면이 아니거나, 위쪽으로 이동할 때
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }

        else // 경사로라면
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force); // 투명 벡터 받아서 슬라이딩!
        }

        if (slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide()
    {
        pm.sliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z); // 원본 크기로 돌아오기
    }
}
