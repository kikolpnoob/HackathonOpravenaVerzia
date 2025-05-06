using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Dash")]
public class DashAbility : Ability
{
    [HideInInspector] public Rigidbody2D rigidBody;
    [HideInInspector] public BossDamageOnContact damageOnContact;
    [HideInInspector] public Collider2D bossCollider;
    [HideInInspector] public Movement movement;
    public float dashVel;
    public int damage;
    public float damageDuration = 0.2f;  // how long you want to deal damage after dash starts

    public override void UseAbility()
    {
        base.UseAbility();
        
        rigidBody.linearVelocity = GetMouseDirection() * dashVel;

        damageOnContact.damage = damage;
        
        Debug.Log(GetMouseDirection() * dashVel);

        damageOnContact.StopCoroutine(DamageWindow());
        damageOnContact.StartCoroutine(DamageWindow());
    }

    private IEnumerator DamageWindow()
    {
        damageOnContact.ClearAlreadyHitList();
        damageOnContact.canDealDamage = true;
        bossCollider.gameObject.layer = LayerMask.NameToLayer("WallCollisionOnly");
        movement.enabled = false;
        yield return new WaitForSeconds(damageDuration);
        movement.enabled = true;
        bossCollider.gameObject.layer = LayerMask.NameToLayer("Boss");
        damageOnContact.canDealDamage = false;
    }

    public Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public Vector2 GetMouseDirection()
    {
        return (GetMousePosition() - (Vector2)Boss.Transform.position).normalized;
    }
}
