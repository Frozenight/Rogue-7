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

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }
}
