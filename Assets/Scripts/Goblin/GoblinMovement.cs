using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMovement : MonoBehaviour
{
    [System.NonSerialized] public SimpleProjectile projectile;
    [SerializeField] private Ability ability; // Array of abilities
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject handPosition;
    private Animator anim;
    private CharacterController controller;
    private float walkingSpeed = 3f; // Speed of the goblin's movement
    private float currentSpeed;
    public float rotationSpeed = 2f; // Speed of the goblin's rotation
    public float jumpForce = 50f; // Force applied to make the goblin jump

    private Vector3 playerVelocity;
    private bool _groundedPlayer;
    private float gravityValue = -9.8f;
    private float distToGround = 0.2f;
    private bool isAttacking = false;
    private bool jumpAttacking = false;
    private float attackInterval = 5f; // Interval for JumpAttack animation
    private float attackTimer = 0f;

    public bool isDead = false;

    public bool groundedPlayer
    {
        get { return _groundedPlayer; }
        set
        {
            if (_groundedPlayer == true && value == false)
            {
            }
            else if (_groundedPlayer == false && value == true)
            {

            }
            _groundedPlayer = value;
        }
    }

    private GoblinAttack attackController;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        attackController = GetComponent<GoblinAttack>();

        currentSpeed = walkingSpeed;
    }

    private void Update()
    {
        if (isDead)
            return;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (isAttacking)
        {
            RotateTowardsTarget();
            return;
        }
        if (jumpAttacking)
        {
            RotateTowardsTarget();
        }

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        controller.Move(transform.forward * currentSpeed * Time.deltaTime);

        attackTimer += Time.deltaTime;

        // Check if it's time to perform an attack
        if (attackTimer >= attackInterval)
        {
            if (Random.Range(0, 2) == 0)
            {
                JumpAnimation();
            }
            else
            {
                NormalAttack();
            }

            attackTimer = 0f;
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

    private void JumpAnimation()
    {
        anim.SetTrigger("JumpAttack");
        Jump();
    }

    private void NormalAttack()
    {
        anim.SetTrigger("SimpleAttack");
        ability.ActivateAbility(this.gameObject, player, handPosition, anim, null);
    }

    private void Jump()
    {
        playerVelocity.y += Mathf.Sqrt(jumpForce * -2f * gravityValue);
    }

    public void SlashAttack()
    {
        StartCoroutine(Stop(3));
        attackController.Attack();
    }

    IEnumerator Stop(float seconds)
    {
        currentSpeed = 0f;
        yield return new WaitForSeconds(seconds);
        currentSpeed = walkingSpeed;
    }

    public void StopMovement()
    {
        isAttacking = true;
    }

    public void ContinueMovement()
    {
        isAttacking = false;
    }

    public void StartJumpRotation()
    {
        jumpAttacking = true;
    }

    public void StopJumpRotation()
    {
        jumpAttacking = false;
    }

    public void ReleaseProejctile()
    {
        projectile.ReleaseProjectile();
    }

    private void RotateTowardsTarget()
    {
        Vector3 targetDirection = player.transform.position - transform.position;
        targetDirection.y = 0; // Set the y-component to zero

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360f * Time.deltaTime);
    }
}
