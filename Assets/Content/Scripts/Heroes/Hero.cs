using System;
using MoreMountains.Feedbacks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Hero : MonoBehaviour
{
    public MMF_Player damagedFeedbacks;
    public ParticleSystem deathParticles;

    public new SpriteRenderer renderer;
    int health;
    public int maxHealth;
    public float movementSpeed;

    public float minDistanceFromPlayer;
    public float maxDistanceFromPlayer;

    protected Rigidbody2D rb;
    [SerializeField] protected float radius;
    
    public bool isUsingAction;

    public float actionStaminaCost;
    // 100 stamina regen per second
    [Header("Hero specific values")]
    float _stamina;

    protected LayerMask targetMask;
    
    public virtual GameObject GetNearestTarget()
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
        
        if (nearestTarget == null)
            return Boss.Transform.gameObject;
        return nearestTarget;
    }
    
    
    public bool isIdealDistance { get { float f = _distanceFromPlayer; return f > minDistanceFromPlayer && f < maxDistanceFromPlayer; } }

    protected float _distanceFromPlayer { get { return Vector2.Distance(GetNearestTarget().transform.position, transform.position); } }
    protected Vector2 _directionToPlayer { get { return (GetNearestTarget().transform.position - transform.position).normalized; } }
    
  

    void Awake()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        targetMask = LayerMask.GetMask("Boss");
    }

    void FixedUpdate()
    {
    }

    protected virtual void UpdateLogic()
    {
        FlipRenderer();
        if (isUsingAction)
            return;
        _stamina = Mathf.Clamp(_stamina + Time.fixedDeltaTime * 100, float.MaxValue, 0);
        if (Mathf.Approximately(_stamina, 0) && isIdealDistance)
        {
            Action();
            _stamina -= actionStaminaCost;
        }
        MoveToPreferredDistance();
    }

    protected virtual void MoveToPreferredDistance()
    {
        float dist = _distanceFromPlayer;
        if (dist < minDistanceFromPlayer)
            rb.AddForce(-_directionToPlayer * movementSpeed * rb.mass);
        else if (dist > maxDistanceFromPlayer)
            rb.AddForce(_directionToPlayer * movementSpeed * rb.mass);
    }
    protected virtual void FlipRenderer()
    {
        renderer.flipX = Vector2.Dot(_directionToPlayer, Vector2.right) < 0; 
    }
    protected virtual void Action()
    {
        // hero specific
    }
    

    public void EditHealth(int value)
    {
        health = Mathf.Clamp(health + value, 0, maxHealth);
        
        if (damagedFeedbacks != null)
            damagedFeedbacks.PlayFeedbacks();
        
        if (health == 0)
            Die();
    }

    protected virtual void Die()
    {
        try { GameController.AddEXP(20); } catch { }
    }
}
