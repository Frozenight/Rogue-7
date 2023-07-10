using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SimpleProjectileAbility")]
public class SimpleProjectileAbility : Ability
{
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] private string releaseTriggerName;
    [SerializeField] private GameObject hitPrefab;

    public override void ActivateAbility(GameObject attacker, GameObject target, GameObject hand, Animator anim, AnimatorEvents animEventController)
    {
        GameObject projectileObject = Instantiate(projectilePrefab, hand.transform.position, Quaternion.identity);
        SimpleProjectile projectile = projectileObject.GetComponent<SimpleProjectile>();

        if(animEventController == null)
        {
            attacker.GetComponent<GoblinMovement>().projectile = projectile;
            projectile.enemyTag = "Player";
        }
        else
        {
            projectile.enemyTag = "Enemy";
        }

        if (projectile != null)
        {
            projectile.SetTarget(target);
            projectile.handPosition = hand;
            projectile.hitVFX = hitPrefab;
            if (animEventController != null)
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
