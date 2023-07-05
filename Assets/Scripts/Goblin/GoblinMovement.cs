using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMovement : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;
    public float speed = 3f; // Speed of the goblin's movement
    public float rotationSpeed = 2f; // Speed of the goblin's rotation
    public float jumpForce = 50f; // Force applied to make the goblin jump

    private Vector3 playerVelocity;
    private bool _groundedPlayer;
    private float gravityValue = -9.8f;
    private float distToGround = 0.2f;
    private bool isAttacking = false;
    private float attackInterval = 5f;
    private float attackTimer = 0f;

    public bool groundedPlayer
    {
        get { return _groundedPlayer; }
        set
        {
            if (_groundedPlayer == true && value == false)
            {
                Debug.Log("In the air");
            }
            else if (_groundedPlayer == false && value == true)
            {
                Debug.Log("Landed");
            }
            _groundedPlayer = value;
        }
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        StartAttacking();
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

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        controller.Move(transform.forward * speed * Time.deltaTime);

        // Check if it's time to perform an attack
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                Attack();
                attackTimer = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround))
        {
            groundedPlayer = true;
        }
        else
        {
            groundedPlayer = false;
        }
    }


    private void Attack()
    {
        anim.SetTrigger("JumpAttack");
        Jump();
    }

    private void Jump()
    {
        Debug.Log(playerVelocity.y);
        playerVelocity.y += Mathf.Sqrt(jumpForce * -2f * gravityValue);
        Debug.Log(playerVelocity.y);
    }

    public void StartAttacking()
    {
        isAttacking = true;
    }

    public void StopAttacking()
    {
        isAttacking = false;
        attackTimer = 0f;
    }
}
