using UnityEngine;

public class AbilityUIHolder : MonoBehaviour
{
    AbilityChoice abilityChoice;
    [HideInInspector] public Ability ability;
    void Start()
    {
        abilityChoice = GetComponentInParent<AbilityChoice>();
    }

    public void ChooseAbility()
    {
        abilityChoice.ChooseAbility(ability);
    }
}