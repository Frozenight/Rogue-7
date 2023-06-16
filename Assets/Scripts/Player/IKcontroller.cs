using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKcontroller : MonoBehaviour
{
    [SerializeField] private PlayerAnimator playerAnimator;

    private void OnAnimatorIK(int layerIndex)
    {
        playerAnimator.FootIK();
    }
}
