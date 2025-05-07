using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Rigidbody2D))]
public class Priest : Hero
{
    public int healAmount;
    private Hero NearestHero;
    public SpriteAnimator spriteAnimator;
    public AudioResource audio;
    public AudioResource deadSound;

    protected override void Action() 
    {
        base.Action();
        isUsingAction = true;
        StartCoroutine(Heal());
    }

    private IEnumerator Heal()
    {
        AudioManager.SpawnAudio(audio, 1);
        spriteAnimator.PlayAnimation("Heal");
        yield return new WaitForSeconds(1f);
        if (NearestHero != null)
            NearestHero.EditHealth(healAmount);
        
        isUsingAction = false;
    }

    public override GameObject GetNearestTarget()
    {
        GameObject nearestTarget = null;
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);
        foreach (Collider2D target in targets)
        {
            if (nearestTarget == null)
                nearestTarget = target.gameObject;
            else if (Vector2.Distance(transform.position, target.transform.position) <
                     Vector2.Distance(transform.position, nearestTarget.transform.position))
            {
                nearestTarget = target.gameObject;
            }
        }

        return nearestTarget;
    }
    
    protected override void Die()
    {
        base.Die();
        StartCoroutine(Dies());
    }

    private IEnumerator Dies()
    {
        rb.linearDamping = 18;
        AudioManager.SpawnAudio(deadSound);
        spriteAnimator.PlayAnimation("Dead");
        Instantiate(deathParticles, transform.position, Quaternion.identity, null).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        UpdateLogic();
        spriteAnimator.PlayAnimation("Walk");
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
