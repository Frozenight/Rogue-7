using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvents : MonoBehaviour
{
    [System.NonSerialized] public SimpleProjectile projectile;
    [SerializeField] Movement playerMovement;
    public void ReleaseProejctile()
    {
        projectile.ReleaseProjectile();
    }

    public void StopMovement()
    {
        playerMovement.StopMovement();
    }

    public void ContinueMovement()
    {
        playerMovement.ContinueMovement();
    }
}
