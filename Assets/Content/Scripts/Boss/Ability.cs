using UnityEngine;

public class Ability : ScriptableObject
{
    public new string name;
    public string description;
    public int manaCost;
    public bool isActive;

    public virtual void UseAbility()
    {
        // Tu niečo bude...
    }
}
