using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public abstract void ActivateAbility(GameObject target, GameObject hand, Animator animator, AnimatorEvents animEventController);

    public void TriggerAnimation(Animator animator, string triggerName)
    {
        animator.SetTrigger(triggerName);
    }
}

