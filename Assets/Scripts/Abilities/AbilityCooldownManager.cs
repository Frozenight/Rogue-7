using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldownManager
{
    private Dictionary<Ability, float> cooldownTimers = new Dictionary<Ability, float>();
    private List<Ability> abilitiesToUpdate = new List<Ability>();
    private Dictionary<Ability, Image> cooldownImages = new Dictionary<Ability, Image>();

    public void StartCooldown(Ability ability, float cooldown, Image cooldownImage)
    {
        if (cooldownTimers.ContainsKey(ability))
        {
            cooldownTimers[ability] = cooldown;
        }
        else
        {
            cooldownTimers.Add(ability, cooldown);
            cooldownImages.Add(ability, cooldownImage);
        }

        cooldownImage.fillAmount = 1f;
    }

    public bool IsOnCooldown(Ability ability)
    {
        return cooldownTimers.ContainsKey(ability) && cooldownTimers[ability] > 0f;
    }

    public void UpdateCooldowns()
    {
        abilitiesToUpdate.Clear();

        foreach (Ability ability in cooldownTimers.Keys)
        {
            if (cooldownTimers[ability] > 0f)
            {
                abilitiesToUpdate.Add(ability);
            }
        }

        foreach (Ability ability in abilitiesToUpdate)
        {
            cooldownTimers[ability] -= Time.deltaTime;
            float fillAmount = cooldownTimers[ability] / ability.Cooldown;
            cooldownImages[ability].fillAmount = fillAmount;

            if (cooldownTimers[ability] < 0f)
            {
                cooldownTimers[ability] = 0f;
                cooldownImages[ability].fillAmount = 0f;
            }
        }
    }
}
