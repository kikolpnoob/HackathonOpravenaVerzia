using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/GroundSlamAbility")]
public class GroundSlamAbility : Ability
{
    [HideInInspector] public Rigidbody2D rigidbody;
    public override void UseAbility()
    {
        base.UseAbility();

        rigidbody.linearVelocity = Vector2.zero;
        Boss.Transform.GetComponent<GroundSlamControler>().StartGroundSlam();
    }
}
