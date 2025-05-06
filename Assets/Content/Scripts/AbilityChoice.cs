using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityChoice : MonoBehaviour
{
    [System.Serializable]
    public class Abilities
    {
        public Sprite abilityIcon;
        public string abilityName;
        public string manaCost;
        public string abilityDescription;
    }

    [System.Serializable]
    public class Carts
    {
        public Image cartIcon;
        public TMP_Text cartName;
        public TMP_Text cartManaCost;
        public TMP_Text cartDescription;
    }
    
    [Header("Abilities Information")]
    public List<Abilities> AbilitieInfo;
    
     [Header("UI Elements")]
    public GameObject selectUI;
    public List<Carts> CartsList;

    public void ClickOnStart()
    {
        selectUI.SetActive(true);
        RandomSetAbility();
    }

    void RandomSetAbility()
    {
        List<Abilities> tempList = new List<Abilities>(AbilitieInfo);

        for (int i = 0; i < CartsList.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, tempList.Count);
            Abilities selected = tempList[randomIndex];

            Carts cart = CartsList[i];
            
            cart.cartIcon.sprite = selected.abilityIcon;
            cart.cartName.text = selected.abilityName;
            cart.cartManaCost.text = selected.manaCost;
            cart.cartDescription.text = selected.abilityDescription;
            

            tempList.RemoveAt(randomIndex);
        }
    }

     public void OnButtonClick()
    {
        GameObject clicked = EventSystem.current.currentSelectedGameObject;
    }
    
    
}
