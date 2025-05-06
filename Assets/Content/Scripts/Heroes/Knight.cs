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
    

    protected override void Action()
    {
        base.Action();
        isUsingAction = true;
        StartCoroutine(Swing());
    }

    void FixedUpdate()
    {
        UpdateLogic();
    }

    private IEnumerator Swing()
    {
        Vector2 swingDirection = (Boss.Transform.position - transform.position).normalized;
        Vector2 swingPosition = (Vector2)transform.position + swingDirection;
        yield return new WaitForSeconds(0.3f);
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