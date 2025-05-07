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
     [Header("UI Elements")]
    public GameObject selectUI;
    public List<Carts> CartsList;

    public void ActivateAbilityChoise()
    {
        selectUI.SetActive(true);
        List<Ability> randomAbilities = RandomSetAbility();
        for (int i = 0; i < 3; i++)
        {
            CartsList[i].cartName.text = randomAbilities[i].name;
            CartsList[i].cartManaCost.text = randomAbilities[i].manaCost.ToString();
            CartsList[i].cartDescription.text = randomAbilities[i].description;
            // TODO: ICON
        }
    }   

    public List<Ability> RandomSetAbility()
    {
        List<Ability> randomAbilities = new List<Ability>();
        bool hasOwned = false;
        foreach (Ability ability in AbilityManager.allAbilities_R)
        {
            foreach (Ability ownedAbility in AbilityManager.ownedAbilities_R)
            {
                if (ability == ownedAbility)
                {
                    hasOwned = true;
                    break;
                }
            }
            
            if (hasOwned)
                continue;

            if (randomAbilities.Count >= 3) 
                break;
            
            randomAbilities.Add(ability);
        }
        
        return randomAbilities;
    }

    public void ChooseAbility(Ability ability)
    {
        AbilityManager.ownedAbilities_R.Add(ability);
        selectUI.SetActive(false);
    }
    
    
}
