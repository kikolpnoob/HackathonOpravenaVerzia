using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [System.Serializable]
    struct AbilityParams
    {
        public Rigidbody2D bossRigidbody;
        public BossDamageOnContact bossDamageOnContact;
        public Collider2D bossCollider;
        public Movement playerMovement;
        public LaserBeamController laserBeamController;
    }
    [SerializeField] AbilityParams abilityParams;
    public List<Ability> ownedAbilities = new List<Ability>();
    public List<Ability> allAbilities = new List<Ability>();

    public float mana;
    public int maxMana;

    private void Awake()
    {
        foreach (Ability ability in allAbilities)
        {
            if (ability is DashAbility dashAbility)
            {
                dashAbility.rigidBody           = abilityParams.bossRigidbody;
                dashAbility.damageOnContact     = abilityParams.bossDamageOnContact;
                dashAbility.movement            = abilityParams.playerMovement;
                dashAbility.bossCollider        = abilityParams.bossCollider;
            }
            if (ability is LaserAbility laserAbility)
            {
                laserAbility.laserBeam          = abilityParams.laserBeamController;
            }
            if (ability is GroundSlamAbility groundSlam)
            {
                groundSlam.rigidbody            = abilityParams.bossRigidbody;
            }
        }

        if (ownedAbilities == null)
            ownedAbilities = new List<Ability>();
        ownedAbilities.Add(allAbilities[0]);
    }


    private void Update()
    {
        mana = Mathf.Clamp(mana + Time.deltaTime * 10, 0, maxMana);
        for (int i = 0; i < 3; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                CheckAbilityActivation(i);
            }
        }
    }

    private void CheckAbilityActivation(int i)
    {
        if (ownedAbilities.Count > i && ownedAbilities[i] != null && mana > ownedAbilities[i].manaCost)
            ownedAbilities[i].UseAbility();
    }
}
