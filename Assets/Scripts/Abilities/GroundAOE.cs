using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAOE : MonoBehaviour
{
    public float radius = 5f;
    public float duration = 2f;

    private Animator animator;
    private string castTriggerName;
    [HideInInspector] public GameObject handPosition;

    private void Update()
    {
        Debug.Log("GroundAOE script");
        transform.position = handPosition.transform.position;
        duration -= Time.deltaTime;
        if (duration <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void Damage()
    {
        Debug.Log("Damage!");
    }

    public void SetAnimator(Animator anim, string triggerName)
    {
        animator = anim;
        castTriggerName = triggerName;
    }

    public void TriggerAnimation()
    {
        animator.SetTrigger(castTriggerName);
    }

    public virtual void Expand()
    {
        // Implementation for the base class
    }
}
