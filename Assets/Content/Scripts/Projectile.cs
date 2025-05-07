using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{

    [HideInInspector] public LayerMask layerMask;
    [HideInInspector] public float speed;
    [HideInInspector] public int damage;
    Rigidbody2D rb;
    public float maxLifeTime = 10.0f;
    public AudioResource shotSound;
    float lifeTime;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = transform.up * speed;
    }

    void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime > maxLifeTime)
            Destroy(gameObject);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        AudioManager.SpawnAudio(shotSound, 5);
        if ((layerMask.value & (1 << other.gameObject.layer)) != 0) // if layerMask contains triggerEnter objects layer
        {
            
            other.GetComponentInParent<Boss>()?.EditHealth(-damage);
            other.GetComponentInParent<Hero>()?.EditHealth(-damage);

            Destroy(gameObject);
        }
    }
}