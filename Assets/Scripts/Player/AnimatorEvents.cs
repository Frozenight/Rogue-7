using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvents : MonoBehaviour
{
    [System.NonSerialized] public SimpleProjectile projectile;
    [System.NonSerialized] public GroundAOE aoe;
    [SerializeField] Movement playerMovement;
    public void ReleaseProejctile()
    {
        projectile.ReleaseProjectile();
    }

    public void AOEeffect()
    {
        aoe.Damage();
    }

    public void StopMovement()
    {
        playerMovement.StopMovement();
    }

    public void ContinueMovement()
    {
        playerMovement.ContinueMovement();
    }

    public void ActivateAOE()
    {
        aoe.Expand();
    }
}
