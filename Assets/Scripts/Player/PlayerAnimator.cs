using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask layerMask;
    [Range(0f, 0.5f)]
    [SerializeField] private float speedAnimationValueFloor = 0.25f;
    [Range(0f, 1f)]
    [SerializeField] private float distToGround;

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

    public void FootIK()
    {
        Debug.Log("Method");
        if (animator)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

            RaycastHit hit;
            Ray ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, distToGround + 1f, layerMask))
            {
                Debug.Log("Raycast");
                if (hit.transform.tag == "Walkable")
                {
                    Vector3 footPosition = hit.point;
                    footPosition.y += distToGround;
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                    Debug.Log("Hit");
                }
            }
        }
    }
}
