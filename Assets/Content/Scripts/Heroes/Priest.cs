using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Priest : Hero
{
    public int healAmount;
    private Hero NearestHero;


    protected override void Action() 
    {
        base.Action();
        isUsingAction = true;
        Heal(); 

    }

    private void Heal()
    {
        if (NearestHero != null)
            NearestHero.EditHealth(healAmount);
        
        isUsingAction = false;
    }

    void FixedUpdate()
    {
        UpdateLogic();
    }

    protected override void UpdateLogic()
    {
        base.UpdateLogic();
    }

    protected override void MoveToPreferredDistance()
    {
        NearestHero = null;
        float nearestAllyDistance = float.MaxValue;
        Vector2 directionToNearestAlly = Vector2.zero; 
   
        foreach (Hero ally in PriestFindAlly.Allies)
        {
            if (ally ==null) continue;
            
            float allyDistance = Vector2.Distance(transform.position, ally.transform.position);
            if (allyDistance < nearestAllyDistance)
            {
                nearestAllyDistance = allyDistance;
                NearestHero = ally;
                directionToNearestAlly = (ally.transform.position - transform.position).normalized;
            }
        }
        
        if (NearestHero != null)
        {
            if (nearestAllyDistance < minDistanceFromPlayer)
                rb.AddForce(-directionToNearestAlly * movementSpeed * rb.mass);
            else if (nearestAllyDistance > maxDistanceFromPlayer)
                rb.AddForce(directionToNearestAlly * movementSpeed * rb.mass);  
        }
        else
        {
            float distan = Vector2.Distance(Boss.Transform.position, transform.position);
            
            if (distan < minDistanceFromPlayer)
                rb.AddForce(-_directionToPlayer * movementSpeed * rb.mass);
            else if (distan > maxDistanceFromPlayer)
                rb.AddForce(_directionToPlayer * movementSpeed * rb.mass);
        }
        

    }
    
  
}
