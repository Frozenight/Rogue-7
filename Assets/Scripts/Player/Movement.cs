using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private float playerSpeed = 2.0f;
    private float sprintSpeedMultiplier = 3f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.8f;
    private float distToGround = 0.2f;
    private float turnSmoothVelocity;

    [Range(0f, 1f)]
    [SerializeField] private float slopeEffectiveAngle;
    public float turnSmoothTime = 0.1f;
    private float slopeAngle;

    private Vector2 moveDirection = Vector2.zero;
    private float currentSpeed;
    private Vector3 lastMoveDir;

    private bool isSprinting = false;

    private InputReader playerInput;
    private PlayerAnimator playerAnimator;

    [SerializeField] private Transform cam;

    private void Awake()
    {
        playerInput = GetComponent<InputReader>();
        controller = GetComponent<CharacterController>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void Start()
    {
        playerInput.OnJump += Jump;
        playerInput.OnSprint += Sprint;

        currentSpeed = 0;
    }

    private void Update()
    {
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Movement
        moveDirection = playerInput.inputVector;
        float targetSpeed = 0;
        Vector3 moveDir = lastMoveDir;

        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            targetSpeed = isSprinting ? playerSpeed * sprintSpeedMultiplier : playerSpeed;
            lastMoveDir = moveDir;
        }

        if (currentSpeed * 1.1 > playerSpeed)
        {
            float transitionSpeed = 3f;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, transitionSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeed = targetSpeed;
        }
        calculateSlope();
        controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        float speedPercent = currentSpeed / (playerSpeed * sprintSpeedMultiplier);
        playerAnimator.Move(speedPercent);
    }


    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround))
        {
            slopeAngle = Vector3.Angle(hit.normal, transform.forward) - 90;
            groundedPlayer = true;
        }
        else
            groundedPlayer = false;
    }

    private void calculateSlope()
    {
        float normalisedSlope = (slopeAngle / 90f) * -1f;
        if (MathF.Abs(normalisedSlope) < slopeEffectiveAngle)
            normalisedSlope = 0;
        currentSpeed += normalisedSlope;
    }


    public void Jump()
    {
        if (groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }

    private void Sprint(bool _isSprinting)
    {
        isSprinting = _isSprinting;
    }
}
