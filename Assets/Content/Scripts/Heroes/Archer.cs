using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Archer : Hero
{
    public SpriteAnimator animator;
    public int damage;
    public LayerMask bossMask;
    public Projectile arrow;
    public float arrowSpeed;
    public float swingSelfKnockback;


    protected override void Action()
    {
        base.Action();
        isUsingAction = true;
        StartCoroutine(Shoot());
    }
    void FixedUpdate()
    {
        UpdateLogic();
    }

    private IEnumerator Shoot()
    {
        Vector2 bossDirection = (Boss.Transform.position - transform.position).normalized;
        Vector2 shootPosition = (Vector2)transform.position + bossDirection;
        yield return new WaitForSeconds(0.3f);
        Projectile projectile = Instantiate(arrow, shootPosition, Quaternion.LookRotation(-Vector3.forward, bossDirection));
        projectile.layerMask = bossMask;
        projectile.damage = damage;
        projectile.speed = arrowSpeed;

        rb.AddForce(-bossDirection * swingSelfKnockback * rb.mass, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.4f);
        isUsingAction = false;
    }
}
