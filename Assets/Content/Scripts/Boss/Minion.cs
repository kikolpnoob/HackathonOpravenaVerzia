using UnityEngine;

public class Minion : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    
    [SerializeField] private float Range = 8f; // Range for enemy detection
    [SerializeField] private float stopDistance = 2f; // Distance from the player at which the minion stops moving

    public LayerMask enemyMask;
    private Transform enemyToFollow;


    [Header("Attacks")]
    public float AttackDelay;
    public float timer;
    public bool CanAttack = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if (GetEnnemyInRange())
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
            rb.AddForce(dirToBoss.normalized * speed);
        }
        else
        {
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
                AttackEnemy();
            }
        }
    }

    public void AttackEnemy()
    {   
        if(CanAttack)
        {
            CanAttack = false;
            Debug.Log("Deal damage to hero");
        }
    }

    public bool GetEnnemyInRange()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, Range, enemyMask);

        if (hit != null)
        {
            Debug.Log("Hit: " + hit.name);
            enemyToFollow = hit.transform;
            return true;
        }
        enemyToFollow = null;
        return false;
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
