using System.Collections;
using MoreMountains.Feedbacks;
using Unity.Mathematics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Rigidbody2D))]
public class Archer : Hero
{
    public SpriteAnimator animator;
    public int damage;
    public LayerMask bossMask;
    public Projectile arrow;
    public float arrowSpeed;
    public float swingSelfKnockback;
    public SpriteAnimator spriteAnimator;

    public List<AudioResource> audios = new List<AudioResource>();
    protected override void Action()
    {
        base.Action();
        isUsingAction = true;
        StartCoroutine(Shoot());
    }
    void FixedUpdate()
    {
        UpdateLogic();
        spriteAnimator.PlayAnimation("Walk");
    }
    
    protected override void Die()
    {
        StartCoroutine(Dies());
    }

    private IEnumerator Dies()
    {
        rb.linearDamping = 18;
        spriteAnimator.PlayAnimation("Dead");
        Instantiate(deathParticles, transform.position, Quaternion.identity, null).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    private IEnumerator Shoot()
    {
        Vector2 bossDirection = (GetNearestTarget().transform.position - transform.position).normalized;
        Vector2 shootPosition = (Vector2)transform.position + bossDirection;
        spriteAnimator.PlayAnimation("Attack");
        AudioManager.SpawnAudio(audios[0], 5);
        
        yield return new WaitForSeconds(1f);
        Projectile projectile = Instantiate(arrow, shootPosition, Quaternion.LookRotation(-Vector3.forward,bossDirection));
        projectile.layerMask = bossMask;
        projectile.damage = damage;
        projectile.speed = arrowSpeed;

        rb.AddForce(-bossDirection * swingSelfKnockback * rb.mass, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.4f);
        isUsingAction = false;
    }
}
