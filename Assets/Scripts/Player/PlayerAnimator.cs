using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Range(0f, 0.5f)]
    [SerializeField] private float speedAnimationValueFloor = 0.25f;

    public void Move(float speedPercent)
    {
        if (speedPercent > 0.05 && speedPercent < speedAnimationValueFloor)
            speedPercent = speedAnimationValueFloor;
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
