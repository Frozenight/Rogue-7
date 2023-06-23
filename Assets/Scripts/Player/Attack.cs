using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject hand;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Ability[] abilities; // Array of abilities
    [SerializeField] private string[] abilityTriggerNames;
    private int currentAbilityIndex = 0; // Index of the currently active ability

    [SerializeField] private Animator anim;
    [SerializeField] private AnimatorEvents animEventController;

    private InputReader playerInput;

    private void Awake()
    {
        playerInput = GetComponent<InputReader>();
    }

    private void Start()
    {
        playerInput.OnAttack += ActivateCurrentAbility;
    }

    private void ActivateCurrentAbility()
    {
        if (GetComponent<Targeting>().GetTarget() == null)
            return;

        GameObject target = GetComponent<Targeting>().GetTarget().gameObject;

        abilities[currentAbilityIndex].ActivateAbility(target, hand, anim, animEventController);

        currentAbilityIndex = (currentAbilityIndex + 1) % abilities.Length;
    }
}
