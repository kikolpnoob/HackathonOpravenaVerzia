using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{ // Coded by kiko

    private Rigidbody2D rb;
    
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
    }

    void FixedUpdate() {

        rb.AddForce(moveDir * speed * rb.mass);
    }
}

//        x horizontal  y vertical