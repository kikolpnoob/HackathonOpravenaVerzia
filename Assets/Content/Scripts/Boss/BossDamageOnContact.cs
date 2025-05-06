using System.Collections.Generic;
using UnityEngine;

public class BossDamageOnContact : MonoBehaviour
{
    [HideInInspector] public int damage;
    [HideInInspector] public bool canDealDamage;

    private List<GameObject> enteredTrigger = new List<GameObject>();
    List<GameObject> alreadyHit = new List<GameObject>();
    void FixedUpdate()
    {
        if (!canDealDamage)
            return;
        for (int i = 0; i < enteredTrigger.Count; i++)
        {
            GameObject g = enteredTrigger[i];
            if (g == null)
            {
                enteredTrigger.RemoveAt(i);
                continue;
            }
            if (alreadyHit.Contains(g))
                continue;
            g.GetComponentInParent<Hero>()?.EditHealth(-damage);
            if (g == null)
                continue;
            alreadyHit.Add(g);
        }
    }

    public void ClearAlreadyHitList()
    {
        alreadyHit.Clear();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (!enteredTrigger.Contains(other.gameObject))
                enteredTrigger.Add(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enteredTrigger.Remove(other.gameObject);
        }
    }
}