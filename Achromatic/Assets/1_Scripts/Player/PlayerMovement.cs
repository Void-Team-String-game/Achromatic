using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;                // �����̴� �ӵ�
    public float walkSpeed;                 // �ȱ� �ӵ�
    public float sprintSpeed;               // �޸��� �ӵ�
    public float slideSpeed;                // �����̵� �ӵ�

    private float desiredMoveSpeed;         
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;   // �����̵��� �ӵ� �߰�
    public float slopeIncreaseMultiplier;   // �����̵��� slope��� �ӵ� �߰�

    public float groundDrag;                // ������ ���� ����

    [Header("Jumping")]
    public float jumpForce;                 // ���� ����(��)
    public float jumpCooldown;              // ���� ��Ÿ��
    public float airMultiplier;             // ���� �ӵ� ����
    bool readyToJump;                       // ���� Ȯ��

    [Header("Crouching")]
    public float crouchSpeed;               // �ɱ� �ӵ�
    public float crouchYScale;              // �ɱ� ũ��
    private float startYScale;              // PlayerObj ó�� ũ��

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;         // ���� Ű
    public KeyCode sprintKey = KeyCode.LeftShift;   // �޸��� Ű
    public KeyCode crouchKey = KeyCode.LeftControl; // �ɱ� Ű

    [Header("Ground Check")]
    public float playerHeight;              // Player ����
    public LayerMask whatIsGround;          // Ground Layer
    bool grounded;                          // �� Ȯ��

    [Header("Slope Handling")]
    public float maxSlopeAngle;             // �ִ� ���� ����
    private RaycastHit slopeHit;            // slopeObj ���� ������ �� ���
    private bool exitingSlope;              // Slope���� ���� ��


    public Transform orientation;           // �÷��̾� ���� ����

    float horizontalInput;                  // A,D �Է�
    float verticalInput;                    // W,S �Է�

    Vector3 moveDirection;                  // �̵� ����

    Rigidbody rb;                           // RigidBody

    public MovementState state;             // �̵� ��� ������
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        sliding,
        air
    }

    public bool sliding;                    // �����̵� üũ

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true; // ó���� ���� True

        startYScale = transform.localScale.y; // ó�� YScale�޾� ����
    }

    private void Update()
    {
        // ground check
        if(state == MovementState.crouching  || state == MovementState.sliding)
        {
            grounded = Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround); // ������ Raycast�� Ȯ��
        }
        else
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround); // ������ Raycast�� Ȯ��
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
        MovePlayer(); // RigidBody�̱� ������ FixUpdate
    }

    private void MyInput()
    {
        // Input System(Manager)���� �� �޾ƿ���
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            // Invoke�� ��Ÿ�� �ð� �� �Լ� ȣ��
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z); // crounchYScale�� ũ�� ����
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse); // ���۽��� ũ�� ����� �Ʒ� ��ġ�� ������ ����, �ٷ� AddForce�� �����ֱ�
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z); // ���� ũ��� ũ�� ����
        }
    }

    private void StateHandler()
    {
        // Mode - Sliding
        if (sliding)
        {
            state = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f) // ���ο� �ְ� �Ʒ��� �̵��ϸ�
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
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 10f && moveSpeed != 0) // ������ �ӵ��� ���ϴ� �ӵ��� ���̰� 4�̻��̶�� Smooth �ڷ�� ����
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else // �ƴ϶�� ��� ���ϴ� �ӵ��� ����
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed); // ���ϴ� �ӵ��� ���� �ӵ��� ����
        float startValue = moveSpeed; // ���� �ӵ��� ���� �ӵ��� ����

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference); // ���� time�� ���� �ü��� ������ �Ǿ� �ڿ������� �ӵ��� ������

            if (OnSlope()) // ���θ�
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal); //Vector3.up�� ���⺤�Ϳ� slopeHit.normal�� �������� ������ ������ ���� ����
                float slopeAngleIncrease = 1 + (slopeAngle / 90f); // 90���� �Ѿ�� 1�� ���ӵ� �߰�

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease; // �̵��ӵ��� �� ������
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier; // ���� �ƴϸ�

            yield return null; // �Ͻ��ߴ� �� ���� �����ӿ��� �ٽ� ����
        }

        moveSpeed = desiredMoveSpeed; // �̵��ӵ��� ���ϴ� �ӵ���
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; // ���� �ִ� ������ �տ� VerticalInput(W, S)�� ���ϰ� ���� �����ʿ� HorizontalInput(A, D)�� �����ֱ�

        // on slope
        if (OnSlope() && !exitingSlope) // �����̰� ���ο��� ������ �ʾҴٸ�
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force); // GetSlopeMoveDirection���� ������ �޾� �Ȱ��� �ӵ��� �ö󰡰� �ϱ�

            if (rb.velocity.y > 0) // ���࿡ ���ο��� ���� Ƥ�ٸ� �Ʒ��� AddForce
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope(); // ������ ���� �� �������°� ����
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope) // ���ο��� �� ���� ���� �����ϱ� ����
        {
            if (rb.velocity.magnitude > moveSpeed) // ���� �ӵ��� moveSpeed���� ������
                rb.velocity = rb.velocity.normalized * moveSpeed; // rb.velocity�� ����ȭ ���� ũ�⸦ 1�� ����� moveSpeed�� ���� ���������� �����.
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // flatVel�̶�� x�� z���� �޴� ���� ����

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed) //flatVel�� �ӵ��� moveSpeed���� ������
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed; // limitedVel�̶�� ���Ϳ� flatVel�� ����ȭ���� 1�� ����� moveSpeed�� ���� ���������� �����.
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); // �׸��� ���������� �����Ŵ
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true; // ���ο��� ���� �ȵǴ� �� ����

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // velocity y�� 0���� �ʱ�ȭ

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); // transform.up�� ���ӿ�����Ʈ�� ȸ���� ����� �ϱ� ������ 
    }
    private void ResetJump() 
    {
        readyToJump = true; 

        exitingSlope = false;
    }

    public bool OnSlope() // ���� Ȯ��
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal); // up ���Ϳ� �������� ������ ���� ����
            return angle < maxSlopeAngle && angle != 0; // maxSlopeAngle���� �۴ٸ� True
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized; // ���� ���Ϳ� �������͸� ���� ���� ������ ������ ����
    }
}