using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [SerializeField] private float cooldown;
    [SerializeField] private float abilitySwitchCooldown;
    public float AbilitySwitchCooldown
    {
        get { return abilitySwitchCooldown; }
    }

    public float Cooldown
    {
        get { return cooldown; }
    }
    public abstract void ActivateAbility(GameObject attacker, GameObject target, GameObject hand, Animator animator, AnimatorEvents animEventController);

    public void TriggerAnimation(Animator animator, string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

}

