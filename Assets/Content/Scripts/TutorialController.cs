using System;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameObject doors;
    private static GameObject _Doors;

    static int dummyDied = 2;

    private void Start()
    {
        _Doors = doors;
    }

    public static void DummyDied()
    {
        dummyDied--;
        if (dummyDied <= 0)
            OpenDoors();
    }

    private static void OpenDoors()
    {
        _Doors.SetActive(false);
    }
}
