using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void Move(float speedPercent)
    {
        animator.SetFloat("SpeedPercent", speedPercent, 0.1f, Time.deltaTime);
    }

    public void IsGrounded(bool isGrounded)
    {
        animator.SetBool("isGrounded", isGrounded);
    }

    public void Fall()
    {
        animator.SetTrigger("Fall");
    }

    public void Fall(float landSpeed)
    {
        landSpeed *= -1;
        if (landSpeed < 4)
            animator.SetTrigger("SoftLand");
        else if (landSpeed < 6)
            animator.SetFloat("Land", 0.1f);
        else
        {
            animator.SetFloat("Land", 1f);
        }
    }

    public void ResetFall()
    {
        animator.ResetTrigger("SoftLand");
    }
}
