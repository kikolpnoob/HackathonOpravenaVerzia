using System;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private SpriteAnimator doorAnimator;
    private static SpriteAnimator _DoorAnimator;
    [SerializeField] private Collider2D doorCollider;
    private static Collider2D _DoorCollider;

    static int dummyDied = 2;

    private void Start()
    {
        _DoorAnimator = doorAnimator;
        _DoorCollider = doorCollider;
    }

    public static void DummyDied()
    {
        dummyDied--;
        if (dummyDied <= 0)
            OpenDoors();
    }

    private static void OpenDoors()
    {
        _DoorAnimator.PlayAnimation("Opened");
        _DoorCollider.enabled = false;
    }
}
