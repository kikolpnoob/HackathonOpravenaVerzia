using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/GroundSlamAbility")]
public class GroundSlamAbility : Ability
{
    [HideInInspector] public Rigidbody2D rigidbody;
    public AudioResource sound;
    public override void UseAbility()
    {
        base.UseAbility();
        AudioManager.SpawnAudio(sound);
        rigidbody.linearVelocity = Vector2.zero;
        Boss.Transform.GetComponent<GroundSlamControler>().StartGroundSlam();
    }
}
