using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{ 

    private Rigidbody2D rb;
    [SerializeField] private new SpriteRenderer renderer;
    [SerializeField] private SpriteAnimator animator;
    
    [Header("Speed")]
    public float speed = 5.0f; 
     
    [Header("Other")]
    private Vector2 moveDir; // Smer pohybu
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update() {
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.y = Input.GetAxisRaw("Vertical"); 
        moveDir = moveDir.normalized;
        

        if (moveDir != Vector2.zero)
            animator.PlayAnimation("Walk");
        else
            animator.PlayAnimation("Idle");

        if (moveDir.x < 0)
            renderer.flipX = true; 
        if (moveDir.x > 0)
            renderer.flipX = false;

    }

    void FixedUpdate() {

        rb.AddForce(moveDir * speed * rb.mass);
    }
}

//        x horizontal  y vertical