using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject hand;
    [SerializeField] private Material handMaterialShader;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Ability[] abilities; // Array of abilities
    [SerializeField] private string[] abilityTriggerNames;
    [SerializeField] private Image[] cooldownImages;
    private int currentAbilityIndex = 0; // Index of the currently active ability
    private int nextAbilityIndex = 0;

    [SerializeField] private Animator anim;
    [SerializeField] private AnimatorEvents animEventController;

    private InputReader playerInput;
    private AbilityCooldownManager cooldownManager;
    private float globalAttackCooldown = 0f;
    private float renderValue = 0f; // Initial value of "_RenderValue"
    private float renderTransitionSpeed = 1f;

    private void Awake()
    {
        playerInput = GetComponent<InputReader>();
        cooldownManager = new AbilityCooldownManager();
    }

    private void Start()
    {
        playerInput.OnAttack += ActivateCurrentAbility;

        foreach (Ability ability in abilities)
        {
            int abilityIndex = System.Array.IndexOf(abilities, ability);
            cooldownManager.StartCooldown(ability, ability.Cooldown, cooldownImages[abilityIndex]);
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
        SetNextAbilityIndex();
        abilities[currentAbilityIndex].ActivateAbility(target, hand, anim, animEventController);
        globalAttackCooldown += currentAbility.AbilitySwitchCooldown;
        int cooldownIndex = System.Array.IndexOf(abilities, currentAbility);
        cooldownManager.StartCooldown(currentAbility, currentAbility.Cooldown, cooldownImages[cooldownIndex]);
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

        Ability currentAbility = abilities[currentAbilityIndex];
        renderValue = cooldownManager.IsOnCooldown(currentAbility) ? 1f : Mathf.MoveTowards(renderValue, 0f, Time.deltaTime * renderTransitionSpeed);
        handMaterialShader.SetFloat("_RenderValue", renderValue);
    }

    private void SetNextAbilityIndex()
    {
        handMaterialShader.SetFloat("_RenderValue", 1);
        switch (currentAbilityIndex)
        {
            case 0:
                handMaterialShader.SetColor("_Color", Color.red);
                return;
            case 1:
                handMaterialShader.SetColor("_Color", Color.cyan);
                return;
            case 2:
                handMaterialShader.SetColor("_Color", Color.white);
                return;
            default:
                handMaterialShader.SetColor("_Color", Color.grey);
                return;
        }
    }
}
