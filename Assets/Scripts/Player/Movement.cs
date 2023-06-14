using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private Coroutine fallCoroutine;
    private bool _groundedPlayer;
    public bool groundedPlayer
    {
        get { return _groundedPlayer; }
        set
        {
            if (_groundedPlayer == true && value == false)
            {
                fallCoroutine = StartCoroutine(startCheckingFall());
                playerAnimator.ResetFall();
            }
            else if (_groundedPlayer == false && value == true)
            {
                if (fallCoroutine != null)
                {
                    StopCoroutine(fallCoroutine);
                    fallCoroutine = null;
                }
                calculateFallSpeed();
            }
            _groundedPlayer = value;
        }
    }

    private float playerSpeed = 2.0f;
    private float sprintSpeedMultiplier = 3f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.8f;
    private float distToGround = 0.2f;
    private float stopPlayerVariable = 1f;
    private float turnSmoothVelocity;

    [Range(0f, 1f)]
    [SerializeField] private float slopeEffectiveAngle;
    [Range(0f, 3f)]
    [SerializeField] private float slopeMultiplier;
    public float turnSmoothTime = 0.1f;
    private float slopeAngle;

    private Vector2 moveDirection = Vector2.zero;
    private float currentSpeed;
    private Vector3 lastMoveDir;
    private Vector3 slopeNormal = Vector3.up;
    private Vector3 moveDirSlope;

    private bool isSprinting = false;
    private bool canMove = true;

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
        if (canMove)
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
            moveDirSlope = moveDir;
        }
        if (currentSpeed * 1.1 > playerSpeed)
        {
            float transitionSpeed = 3f;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, transitionSpeed * Time.deltaTime);
        }
        else
        {
            moveDirSlope = Vector3.zero;
            currentSpeed = targetSpeed;
        }
        calculateSlope();
        if (groundedPlayer)
        {
            moveDir = Vector3.ProjectOnPlane(moveDir, slopeNormal);
        }
        controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime * stopPlayerVariable);
        float speedPercent = currentSpeed / (playerSpeed * sprintSpeedMultiplier);
        playerAnimator.Move(speedPercent);
        Debug.Log(currentSpeed);
    }



    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround))
        {
            slopeNormal = hit.normal;
            slopeAngle = Vector3.Angle(hit.normal, lastMoveDir) - 90;
            groundedPlayer = true;
            playerAnimator.IsGrounded(true);
        }
        else
        {
            slopeNormal = Vector3.up;
            groundedPlayer = false;
            playerAnimator.IsGrounded(false);
        }
    }

    private void calculateSlope()
    {
        float normalisedSlope = (slopeAngle / 90f) * -1f;
        if (MathF.Abs(normalisedSlope) < slopeEffectiveAngle)
            normalisedSlope = 0;

        float slopeSpeedMultiplier = 1f + (normalisedSlope * slopeMultiplier);
        float targetSpeed = isSprinting ? playerSpeed * sprintSpeedMultiplier : playerSpeed;
        float transitionSpeed = 3f;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed * slopeSpeedMultiplier, transitionSpeed * Time.deltaTime);
    }



    private void calculateFallSpeed()
    {
        playerAnimator.Fall(playerVelocity.y);
        calculateLandTime(playerVelocity.y);
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

    private void calculateLandTime(float landSpeed)
    {
        landSpeed *= -1;
        if (landSpeed < 4)
            return;
        else if (landSpeed < 6)
            StartCoroutine(pauseMovement(0.0f, true));
        else
        {
            StartCoroutine(pauseMovement(1.53f, false));
        }
    }

    private IEnumerator pauseMovement(float pauseTime, bool stopPlayer)
    {
        canMove = false;
        if (stopPlayer)
        {
            stopPlayerVariable = 0;
            yield return new WaitForSeconds(pauseTime);
        }
        else
        {
            float elapsedTime = 0f;
            while (elapsedTime < pauseTime)
            {
                controller.Move(lastMoveDir * 1.5f * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        stopPlayerVariable = 1;
        canMove = true;
    }

    private IEnumerator startCheckingFall()
    {
        while (true)
        {
            if (playerVelocity.y > 3 || playerVelocity.y < -3)
            {
                playerAnimator.Fall();
                yield break;
            }

            yield return null;
        }
    }
}
