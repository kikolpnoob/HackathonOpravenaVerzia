using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PriestFindAlly : MonoBehaviour
{
    public static List<Hero> Allies =  new List<Hero>();

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Allies.Add(other.gameObject.GetComponentInParent<Hero>());
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Allies.Remove(other.gameObject.GetComponentInParent<Hero>());
        }
    }
}
