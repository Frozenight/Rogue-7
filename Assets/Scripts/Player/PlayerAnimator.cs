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
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("IKLeftFootWeight"));
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("IKLeftFootWeight"));
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, animator.GetFloat("IKRightFootWeight"));
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, animator.GetFloat("IKRightFootWeight"));

        // Left Foot
        RaycastHit hit;
        Ray ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out hit, distToGround + 1f, layerMask))
        {
            if (hit.transform.CompareTag("Ground"))
            {
                Vector3 footPosition = hit.point;
                footPosition.y += distToGround;
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                Vector3 forward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(forward, hit.normal));
            }
        }

        // Right Foot
        ray = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out hit, distToGround + 1f, layerMask))
        {
            if (hit.transform.CompareTag("Ground"))
            {
                Vector3 footPosition = hit.point;
                footPosition.y += distToGround;
                animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
                Vector3 forward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(forward, hit.normal));
            }
        }
    }
}
