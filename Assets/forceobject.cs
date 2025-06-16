using UnityEngine;

public class PushableObject : MonoBehaviour
{
    public float pushForce = 100f;
    public Vector2 pushDirection = new Vector2(1, 1);
    public float gravityReduceRate = 1f; 
    public float minGravityScale = 0.1f;

    private Rigidbody2D rb;
    private bool isPushed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pushDirection.Normalize();
    }

    public void ApplyPush(float directionMultiplier)
    {
        Vector2 force = new Vector2(pushDirection.x * directionMultiplier, pushDirection.y) * pushForce;
        rb.velocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
        isPushed = true;
    }

    void Update()
    {
        if (isPushed && rb.gravityScale > minGravityScale)
        {
            rb.gravityScale -= gravityReduceRate * Time.deltaTime;
            if (rb.gravityScale < minGravityScale)
            {
                rb.gravityScale = minGravityScale;
            }
        }
    }
}
