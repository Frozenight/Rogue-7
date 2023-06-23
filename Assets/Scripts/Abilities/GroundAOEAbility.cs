using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/GroundAOEAbility")]
public class GroundAOEAbility : Ability
{
    [SerializeField] public GameObject aoePrefab;
    [SerializeField] private string castTriggerName;

    public override void ActivateAbility(GameObject target, GameObject hand, Animator anim, AnimatorEvents animEventController)
    {
        GameObject aoeObject = Instantiate(aoePrefab, hand.transform.position, Quaternion.identity);
        GroundAOE aoe = aoeObject.GetComponent<GroundAOE>();

        if (aoe != null)
        {
            aoe.handPosition = hand;
            aoe.SetAnimator(anim, castTriggerName);
            aoe.TriggerAnimation();
            animEventController.aoe = aoe;
        }
        else
        {
            Debug.LogError("Ground AOE component not found on the instantiated object!");
            Destroy(aoeObject);
        }
    }
}
