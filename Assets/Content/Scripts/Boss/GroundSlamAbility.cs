using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/GroundSlamAbility")]
public class GroundSlamAbility : Ability
{
    public override void UseAbility()
    {
        base.UseAbility();

        Boss.Transform.GetComponent<GroundSlamControler>().StartGroundSlam();
    }
}
