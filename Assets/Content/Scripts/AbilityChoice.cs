using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityChoice : MonoBehaviour
{
    [System.Serializable]
    public struct Carts
    {
        public Image cartIcon;
        public TMP_Text cartName;
        public TMP_Text cartManaCost;
        public TMP_Text cartDescription;
    }
    public AbilityManager abilityManager;
    [Header("UI Elements")]
    public GameObject selectUI;
    public List<Carts> CartsList;

    public void ActivateAbilityChoise()
    {
        selectUI.SetActive(true);
        List<Ability> randomAbilities = GetAbilityChoices();
        for (int i = 0; i < 3; i++)
        {
            CartsList[i].cartName.text = randomAbilities[i].name;
            CartsList[i].cartManaCost.text = randomAbilities[i].manaCost.ToString();
            CartsList[i].cartDescription.text = randomAbilities[i].description;
            // TODO: ICON
        }
    }   

    public List<Ability> GetAbilityChoices()
    {
        List<Ability> randomAbilities = new List<Ability>();
        foreach (Ability potentialAbility in abilityManager.allAbilities)
        {
            if (abilityManager.ownedAbilities.Contains(potentialAbility))
                continue;

            if (randomAbilities.Count >= 3) 
                break;
            
            randomAbilities.Add(potentialAbility);
        }
        
        return randomAbilities;
    }

    public void ChooseAbility(Ability ability)
    {
        abilityManager.ownedAbilities.Add(ability);
        selectUI.SetActive(false);
    }
    
    
}
