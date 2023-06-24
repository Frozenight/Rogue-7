using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject hand;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Ability[] abilities; // Array of abilities
    [SerializeField] private string[] abilityTriggerNames;
    private int currentAbilityIndex = 0; // Index of the currently active ability
    private int nextAbilityIndex = 0;

    [SerializeField] private Animator anim;
    [SerializeField] private AnimatorEvents animEventController;

    private InputReader playerInput;
    [SerializeField] private Image[] cooldownImages;
    private AbilityCooldownManager cooldownManager;
    private float globalAttackCooldown = 0f;

    private void Awake()
    {
        playerInput = GetComponent<InputReader>();
        cooldownManager = new AbilityCooldownManager();
    }

    private void Start()
    {
        playerInput.OnAttack += ActivateCurrentAbility;

        for (int i = 0; i < abilities.Length; i++)
        {
            cooldownManager.StartCooldown(abilities[i], abilities[i].Cooldown, cooldownImages[i]);
        }
    }

    private void ActivateCurrentAbility()
    {
        if (GetComponent<Targeting>().GetTarget() == null || globalAttackCooldown > 0f)
            return;
        currentAbilityIndex = nextAbilityIndex;
        GameObject target = GetComponent<Targeting>().GetTarget().gameObject;
        Ability currentAbility = abilities[currentAbilityIndex];
        if (cooldownManager.IsOnCooldown(currentAbility))
        {
            Debug.Log("Ability is on cooldown!");
            return;
        }

        abilities[currentAbilityIndex].ActivateAbility(target, hand, anim, animEventController);
        globalAttackCooldown += abilities[currentAbilityIndex].AbilitySwitchCooldown;
        cooldownManager.StartCooldown(currentAbility, currentAbility.Cooldown, cooldownImages[currentAbilityIndex]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            nextAbilityIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            nextAbilityIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            nextAbilityIndex = 2;
        }

        if (globalAttackCooldown > 0f)
            globalAttackCooldown -= Time.deltaTime;
        cooldownManager.UpdateCooldowns();
    }
}
