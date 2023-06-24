using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SimpleProjectileAbility")]
public class SimpleProjectileAbility : Ability
{
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] private string releaseTriggerName;
    [SerializeField] private GameObject hitPrefab;

    public override void ActivateAbility(GameObject target, GameObject hand, Animator anim, AnimatorEvents animEventController)
    {
        GameObject projectileObject = Instantiate(projectilePrefab, hand.transform.position, Quaternion.identity);
        SimpleProjectile projectile = projectileObject.GetComponent<SimpleProjectile>();

        if (projectile != null)
        {
            projectile.SetTarget(target.transform.position);
            projectile.handPosition = hand;
            projectile.hitVFX = hitPrefab;
            animEventController.projectile = projectile;
            projectile.SetAnimator(anim, releaseTriggerName);
            projectile.TriggerAnimation();
        }
        else
        {
            Debug.LogError("Projectile component not found on the instantiated object!");
            Destroy(projectileObject);
        }
    }
}
