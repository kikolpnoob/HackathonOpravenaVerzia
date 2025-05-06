using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;



public class Boss : MonoBehaviour
{
    int health;
    public int maxHealth;
    public int xp;
    public static Transform Transform;
    
    public float meeleAttackRadius;
    public LayerMask enemyLayerMask;
    
    public int damage; // TODO: ...

    public float delay = 1;
    public bool CanAttack = true;
    public float timer = 0;
    
    public Collider2D[] enemies;


    
    private void Start()
    {
        Transform = transform;
        health = maxHealth;
    }
    
    private void Update()
    {
        if (!CanAttack)
        {
            timer += Time.deltaTime;

            if (timer >= delay)
            {
                CanAttack = true;
                timer = 0;
            }
        }
        
        if (Input.GetMouseButtonDown(0) && CanAttack)
        {
            enemies = Physics2D.OverlapCircleAll(GetSwingPosition(), meeleAttackRadius, enemyLayerMask); // Toto nejde ... stale to je null
            CanAttack = false;
            if (enemies != null)
            {
                foreach (Collider2D enemy in enemies)
                {
                    //TODO: Ubere Demage 
                   // enemy.GetComponentInParent<Hero>().EditHealth(-damage);
                }
            }
        }
    }
    public Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public Vector2 GetSwingPosition()
    {
        return (Vector2)transform.position + (GetMousePosition() - (Vector2)transform.position).normalized;
    }
    
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(GetSwingPosition(), Vector3.forward, meeleAttackRadius);
    }

    public void EditHealth(int value)
    {
        health = Mathf.Clamp(health + value, 0, maxHealth);

        Debug.Log(health);
        if (health == 0)
            Destroy(gameObject); // gameOver
    }
}