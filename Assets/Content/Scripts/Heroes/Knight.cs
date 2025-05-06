using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;

public class Knight : Hero
{
    public int damage;
    public LayerMask bossMask;
    [Tooltip("Radius of the damage collider")]
        public Vector2 swingSize;
    [Tooltip("Distance of the swing from the hero")]
        public float swingDistance;
    public float swingSelfKnockback;
    public SpriteAnimator spriteAnimator;
    

    protected override void Action()
    {
        base.Action();
        isUsingAction = true;
        StartCoroutine(Swing());
    }

    protected override void Die()
    {
        StartCoroutine(Dies());
    }

    private IEnumerator Dies()
    {
        spriteAnimator.PlayAnimation("Dead");
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        UpdateLogic();
        spriteAnimator.PlayAnimation("Walk");
    }

    private IEnumerator Swing()
    {
        Vector2 swingDirection = (Boss.Transform.position - transform.position).normalized;
        Vector2 swingPosition = (Vector2)transform.position + swingDirection;
        spriteAnimator.PlayAnimation("Attack");
        yield return new WaitForSeconds(1f);
        Collider2D col = Physics2D.OverlapBox(swingPosition, swingSize, Vector2.Angle(Vector2.up, swingDirection), bossMask);
        if (col != null)
        {
            col.GetComponentInParent<Boss>().EditHealth(-damage);
            // Debug.Log("Hit da boss");
        }
        rb.AddForce(-swingDirection * swingSelfKnockback * rb.mass, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.4f);
        isUsingAction = false;
    }
}