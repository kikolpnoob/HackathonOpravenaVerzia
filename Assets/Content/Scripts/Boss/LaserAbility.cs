using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/LaserAbility")]
public class LaserAbility : Ability
{
    [HideInInspector] public LaserBeamController laserBeam;
    public AudioResource sound;
    public override void UseAbility()
    {
        base.UseAbility();

        if (!laserBeam.isFiring)
            AudioManager.SpawnAudio(sound);
            laserBeam.StartLaser();
    }
}
