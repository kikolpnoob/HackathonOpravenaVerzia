using UnityEngine;

public class GroundSlamControler : MonoBehaviour
{
    public float GroundSlamRadius = 3f;
    public LayerMask enemyLayerMask;
    public int damage = 20;

    private bool CanAttack = false;

    public ParticleSystem groundSlamParticles;

    void Update()
    {
        if (CanAttack)
        {

            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, GroundSlamRadius, enemyLayerMask);
            groundSlamParticles.Play();

            //Debug.Log($"Ground slam at {transform.position}, found {enemies.Length} enemies.");

            foreach (Collider2D enemy in enemies)
            {
                Hero hero = enemy.GetComponentInParent<Hero>();
                if (hero != null)
                {
                    hero.EditHealth(-damage);
                    Debug.Log("Dealt damage WITH Slam to: " + hero.name);
                }
            }

            CanAttack = false;
        }
    }

    public void StartGroundSlam()
    {
        CanAttack = true;
    }

    public Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, GroundSlamRadius);
    }
}
