using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/LaserAbility")]
public class LaserAbility : Ability
{
    [HideInInspector] public LaserBeamController laserBeam;
    public override void UseAbility()
    {
        base.UseAbility();

        if (!laserBeam.isFiring)
            laserBeam.StartLaser();
    }
}
