using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected PlayerData data = null;
    [SerializeField] protected Rigidbody2D rb = null;

    protected void SetVelocity(int angle)
    {
        transform.eulerAngles = Vector3.forward * angle;
        rb.velocity = transform.right * data.bulletSpeed;
    }

    private void OnCollisionExit2D(Collision2D _)
    {
        rb.velocity = rb.velocity.normalized * data.bulletSpeed; // To ensure the bullet never slows down off of odd collisions
    }
}
