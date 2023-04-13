using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;                   // ����
    public Transform playerObj;                     // PlayerObj
    private Rigidbody rb;                           // RigidBody
    private PlayerMovement pm;                      // PlayerMovement ��ũ��Ʈ

    [Header("Sliding")]
    public float maxSlideTime;                      // �ִ� �����̵� �ð�
    public float slideForce;                        // �����̵� ��
    private float slideTimer;                       // �ð� Ÿ�̸�

    public float slideYScale;                       // Sliding�� YScale
    private float startYScale;                      // ���۽� YScale

    [Header("Input")]
    public KeyCode Slidekey = KeyCode.LeftControl;  // �����̵� Ű
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

        if (Input.GetKeyDown(Slidekey) && (horizontalInput != 0 || verticalInput != 0)) // �ȿ����̴� ���¿��� �����̵� ���ϰ� ����
            StartSlide();

        if (Input.GetKeyUp(Slidekey) && pm.sliding)
            StopSlide();
    }

    private void StartSlide()
    {
        pm.sliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z); // �÷��̾� ���� ����
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse); // ���� ���� ���� �Ʒ��� �о��ֱ�

        slideTimer = maxSlideTime; // Timer�� maxTime���� ����
    }

    private void FixedUpdate()
    {
        if (pm.sliding) // �����̵���
            SlidingMovement();
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; // ���� �ִ� ������ �տ� VerticalInput(W, S)�� ���ϰ� ���� �����ʿ� HorizontalInput(A, D)�� �����ֱ�

        //sliding normal
        if (!pm.OnSlope() || rb.velocity.y > -0.1f) // ������ �ƴϰų�, �������� �̵��� ��
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }

        else // ���ζ��
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force); // ���� ���� �޾Ƽ� �����̵�!
        }

        if (slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide()
    {
        pm.sliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z); // ���� ũ��� ���ƿ���
    }
}
