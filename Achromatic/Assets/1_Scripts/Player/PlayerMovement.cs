using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;                // 움직이는 속도
    public float walkSpeed;                 // 걷기 속도
    public float sprintSpeed;               // 달리기 속도
    public float slideSpeed;                // 슬라이딩 속도

    private float desiredMoveSpeed;         
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;   // 슬라이딩시 속도 추가
    public float slopeIncreaseMultiplier;   // 슬라이딩시 slope라면 속도 추가

    public float groundDrag;                // 마찰력 조절 변수

    [Header("Jumping")]
    public float jumpForce;                 // 점프 높이(힘)
    public float jumpCooldown;              // 점프 쿨타임
    public float airMultiplier;             // 공중 속도 조절
    bool readyToJump;                       // 점프 확인

    [Header("Crouching")]
    public float crouchSpeed;               // 앉기 속도
    public float crouchYScale;              // 앉기 크기
    private float startYScale;              // PlayerObj 처음 크기

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;         // 점프 키
    public KeyCode sprintKey = KeyCode.LeftShift;   // 달리기 키
    public KeyCode crouchKey = KeyCode.LeftControl; // 앉기 키

    [Header("Ground Check")]
    public float playerHeight;              // Player 높이
    public LayerMask whatIsGround;          // Ground Layer
    bool grounded;                          // 땅 확인

    [Header("Slope Handling")]
    public float maxSlopeAngle;             // 최대 기울기 각도
    private RaycastHit slopeHit;            // slopeObj 정보 가져올 때 사용
    private bool exitingSlope;              // Slope에서 나올 때


    public Transform orientation;           // 플레이어 보는 방향

    float horizontalInput;                  // A,D 입력
    float verticalInput;                    // W,S 입력

    Vector3 moveDirection;                  // 이동 방향

    Rigidbody rb;                           // RigidBody

    public MovementState state;             // 이동 방식 열거형
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        sliding,
        air
    }

    public bool sliding;                    // 슬라이딩 체크

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true; // 처음에 점프 True

        startYScale = transform.localScale.y; // 처음 YScale받아 오기
    }

    private void Update()
    {
        // ground check
        if(state == MovementState.crouching  || state == MovementState.sliding)
        {
            grounded = Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround); // 땅인지 Raycast로 확인
        }
        else
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround); // 땅인지 Raycast로 확인
        Debug.DrawRay(transform.position - new Vector3(0, 0.1f, 0), Vector3.down, Color.red);
        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround))
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer(); // RigidBody이기 떄문에 FixUpdate
    }

    private void MyInput()
    {
        // Input System(Manager)으로 값 받아오기
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            // Invoke로 쿨타임 시간 훈 함수 호출
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z); // crounchYScale로 크기 변경
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse); // 갑작스런 크기 변경시 아래 위치에 공간이 남음, 바로 AddForce로 내려주기
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z); // 원래 크기로 크기 변경
        }
    }

    private void StateHandler()
    {
        // Mode - Sliding
        if (sliding)
        {
            state = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f) // 경사로에 있고 아래로 이동하면
                desiredMoveSpeed = slideSpeed;

            else
                desiredMoveSpeed = sprintSpeed;
        }

        // Mode - Crouching
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }

        // check if desiredMoveSpeed has changed drastically
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 10f && moveSpeed != 0) // 마지막 속도와 원하는 속도의 차이가 4이상이라면 Smooth 코루딘 실행
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else // 아니라면 즉시 원하는 속도로 변경
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed); // 원하는 속도와 현재 속도의 차이
        float startValue = moveSpeed; // 시작 속도에 현재 속도에 대입

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference); // 점차 time의 값이 늘수록 보간이 되어 자연스럽게 속도가 증가함

            if (OnSlope()) // 경사로면
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal); //Vector3.up의 방향벡터와 slopeHit.normal의 법선벡터 사이의 각도를 구해 저장
                float slopeAngleIncrease = 1 + (slopeAngle / 90f); // 90도를 넘어가면 1을 가속도 추가

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease; // 이동속도를 더 높혀줌
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier; // 경사로 아니면

            yield return null; // 일시중단 후 다음 프레임에서 다시 시작
        }

        moveSpeed = desiredMoveSpeed; // 이동속도를 원하는 속도로
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; // 보고 있는 방향의 앞에 VerticalInput(W, S)값 곱하고 방향 오르쪽에 HorizontalInput(A, D)에 곱해주기

        // on slope
        if (OnSlope() && !exitingSlope) // 경사로이고 경사로에서 나가지 않았다면
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force); // GetSlopeMoveDirection으로 방향을 받아 똑같은 속도로 올라가게 하기

            if (rb.velocity.y > 0) // 만약에 경사로에서 통통 튄다면 아래로 AddForce
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope(); // 가만히 있을 떄 내려오는거 방지
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope) // 경사로에서 더 빠른 것을 방지하기 위해
        {
            if (rb.velocity.magnitude > moveSpeed) // 현재 속도가 moveSpeed보다 빠르면
                rb.velocity = rb.velocity.normalized * moveSpeed; // rb.velocity를 정규화 시켜 크기를 1로 만들고 moveSpeed를 곱해 정상적으로 만든다.
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // flatVel이라는 x와 z값을 받는 벡터 생성

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed) //flatVel의 속도가 moveSpeed보다 빠르면
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed; // limitedVel이라는 벡터에 flatVel을 정규화시켜 1로 만들고 moveSpeed를 곱해 정상적으로 만든다.
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); // 그리고 정상적으로 적용시킴
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true; // 경사로에서 점프 안되는 거 방지

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // velocity y를 0으로 초기화

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); // transform.up은 게임오브젝트의 회전도 고려를 하기 때문에 
    }
    private void ResetJump() 
    {
        readyToJump = true; 

        exitingSlope = false;
    }

    public bool OnSlope() // 경사로 확인
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal); // up 벡터와 법선벡터 사이의 각도 저장
            return angle < maxSlopeAngle && angle != 0; // maxSlopeAngle보다 작다면 True
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized; // 방향 벡터와 법선벡터를 통해 투영 벡터의 방향을 리턴
    }
}