using System.Collections;
using UnityEngine;

public class Dummy : Hero
{
    public SpriteAnimator spriteAnimator;
    protected override void Die()
    {
        TutorialController.DummyDied();
        StartCoroutine(Dies());
    }

    private IEnumerator Dies()
    {
        spriteAnimator.PlayAnimation("Dead");
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
