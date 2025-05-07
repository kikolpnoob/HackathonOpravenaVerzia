using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Audio;

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
    public AudioResource audio;
    public AudioResource deadSound;
    

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
        rb.linearDamping = 18;
        AudioManager.SpawnAudio(deadSound);
        spriteAnimator.PlayAnimation("Dead");
        Instantiate(deathParticles, transform.position, Quaternion.identity, null).gameObject.SetActive(true);
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
        Vector2 swingDirection = (GetNearestTarget().transform.position - transform.position).normalized;
        Vector2 swingPosition = (Vector2)transform.position + swingDirection;
        spriteAnimator.PlayAnimation("Attack");
        AudioManager.SpawnAudio(audio);
        yield return new WaitForSeconds(0.375f);
        Collider2D col = Physics2D.OverlapBox(swingPosition, swingSize, Vector2.Angle(Vector2.up, swingDirection), bossMask);
        if (col != null)
        {
            if(col.GetComponentInParent<Boss>())
                col.GetComponentInParent<Boss>().EditHealth(-damage);
            if(col.GetComponentInParent<Minion>())
                col.GetComponentInParent<Minion>().EditHealth(-damage);
            // Debug.Log("Hit da boss");
        }
        rb.AddForce(-swingDirection * swingSelfKnockback * rb.mass, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.625f);
        isUsingAction = false;
    }
}