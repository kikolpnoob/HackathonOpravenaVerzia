using UnityEngine;

public class Dummy : Hero
{
    protected override void Die()
    {
        TutorialController.DummyDied();
        base.Die();
    }
}
