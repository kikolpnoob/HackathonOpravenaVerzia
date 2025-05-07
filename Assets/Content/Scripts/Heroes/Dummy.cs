using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Dummy : Hero
{
    public SpriteAnimator spriteAnimator;
    public AudioResource deadSound;
    
    protected override void Die()
    {
        TutorialController.DummyDied();
        StartCoroutine(Dies());
    }

    private IEnumerator Dies()
    {
        AudioManager.SpawnAudio(deadSound);
        spriteAnimator.PlayAnimation("Dead");
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
