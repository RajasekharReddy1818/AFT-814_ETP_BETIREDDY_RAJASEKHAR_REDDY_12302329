using UnityEngine;

public class PushableObject : MonoBehaviour
{
    public float pushForce = 50f; 
    public Vector2 pushDirection = new Vector2(1f, 1f); 

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (pushDirection == Vector2.zero)
        {
            pushDirection = new Vector2(1f, 1f); 
        }
        pushDirection.Normalize(); 
    }

    public void ApplyPush(float directionMultiplier)
    {
        Vector2 force = new Vector2(pushDirection.x * directionMultiplier, pushDirection.y) * pushForce;
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}
