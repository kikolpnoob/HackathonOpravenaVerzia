using UnityEngine;

public class LaserBeamController : MonoBehaviour
{
    public LineRenderer linRen;
    public LayerMask hitLayers;
    public float laserLength = 100f;
    public int damagePerTick = 5;
    public bool isFiring = false;
    public float duration = 0.5f;
    private float timer;
    private float tickTimer;

    private Vector3 laserDirection;

    void Start()
    {
        linRen = GetComponent<LineRenderer>();
        if (linRen != null)
            linRen.enabled = false;

    }

    void Update()
    {
        if(isFiring)
        {
            timer += Time.deltaTime * 1;
            if(timer > duration)
            {
                timer = 0;
                StopLaser();
            }
        }
        laserDirection = (GetMousePosition() - (Vector2)Boss.Transform.position).normalized;
    }

    void FixedUpdate()
    {
        if (!isFiring)
        {
            linRen.enabled = false;
            return;
        }

        Vector3 start = transform.position;


        // 1. Draw beam
        linRen.SetPosition(0, start);
        linRen.SetPosition(1, start + laserDirection * laserLength);
        linRen.enabled = true;

        tickTimer -= Time.fixedDeltaTime;
        if (tickTimer > 0)
        {
            return;
        }
        tickTimer += 0.1f;
        // 2. Hit everything on the laser path
        RaycastHit2D[] hits = Physics2D.RaycastAll(start, laserDirection, laserLength, hitLayers);

        foreach (RaycastHit2D hit in hits)
        {
            Hero hp = hit.collider.GetComponentInParent<Hero>();
            if (hp != null)
            {
                hp.EditHealth(-damagePerTick);
            }
        }
    }

    public void StartLaser()
    {
        isFiring = true;
    }

    public void StopLaser()
    {
        isFiring = false;
    }
    public Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}

