using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Minion : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    
    [SerializeField] private float Range = 8f; // Range for enemy detection
    [SerializeField] private float stopDistance = 2f; // Distance from the player at which the minion stops moving

    public LayerMask enemyMask;
    private Transform enemyToFollow;
    private Collider2D enemy;
    
    int health;
    public int maxHealth;

    public SpriteAnimator animator;

    [Header("Attacks")]
    public float AttackDelay;
    public float timer;
    public bool CanAttack = true;
    public int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    public void Update()
    {
        if (GetEnemyInRange())
        {
            FollowEnemy();
        }
        else
        {
            FollowPlayer();
        }

        if (!CanAttack)
        {
            timer += Time.deltaTime;

            if (timer >= AttackDelay)
            {
                CanAttack = true;
                timer = 0f;
            }
        }
    }

    public void FollowPlayer()
    {
        Vector2 dirToBoss = Boss.Transform.position - transform.position;

        if (dirToBoss.magnitude > stopDistance)
        {
            animator.PlayAnimation("Walk");
            rb.AddForce(dirToBoss.normalized * speed);
        }
        else
        {
            animator.PlayAnimation("Idle");
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void FollowEnemy()
    {
        if (enemyToFollow != null)
        {
            
            Vector2 dirToEnemy = enemyToFollow.position - transform.position;

            if (dirToEnemy.magnitude > stopDistance)
            {
                rb.AddForce(dirToEnemy.normalized * speed);
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                animator.PlayAnimation("Idle");
                if (!isInCoroutine) 
                    StartCoroutine(AttackEnemy());
            }
        }
    }

    private bool isInCoroutine; 
    public IEnumerator AttackEnemy()
    {   
        isInCoroutine = true;
            animator.PlayAnimation("Attack");
            yield return new WaitForSeconds(0.5f);

            if (enemy != null)
            {
                enemy.gameObject.GetComponentInParent<Hero>().EditHealth(-damage);
            }

            isInCoroutine = false;
    }

    public bool GetEnemyInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, Range, enemyMask);
        Collider2D hit = null;
        
        if (hits.Length > 0) 
            hit = hits[0];
        

        foreach (Collider2D hitForeach in hits)
        {
            if (Vector2.Distance(transform.position, hitForeach.transform.position) < Vector2.Distance(transform.position, hit.transform.position))
            {
                hit = hitForeach;
            }
        }

        if (hit != null)
        {
            enemyToFollow = hit.transform;
            enemy = hit.GetComponent<Collider2D>();
            return true;
        }
        enemyToFollow = null;
        return false;
    }
    public void EditHealth(int value)
    {
        health = Mathf.Clamp(health + value, 0, maxHealth);
        
        if (health == 0)
            Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
    

    void OnDrawGizmos()
    {
        // Draw range for the enemy detection
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
        
        // Draw stop range for the player
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
