using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Summon")]
public class SummonMinions : Ability
{
    public GameObject MinionPrefab;
    public AudioResource MinionAudio;
    
    public override void UseAbility()
    {
        base.UseAbility();
        // AudioManager.SpawnAudio(MinionAudio);
        Object.Instantiate(MinionPrefab, Boss.Transform.position, Quaternion.identity);
    }

    
}
