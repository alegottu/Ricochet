using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected static readonly Vector2Int bulletAngleRange = new Vector2Int(75, 105);
    protected const float speed = 15;

    [SerializeField] protected Rigidbody2D rb = null;
    [SerializeField] protected AudioSource sfx = null;

    protected void SetVelocity(int angle)
    {
        transform.eulerAngles = Vector3.forward * angle;
        rb.velocity = transform.right * speed;
    }

    private void OnCollisionExit2D(Collision2D _)
    {
        rb.velocity = rb.velocity.normalized * speed; // To ensure the bullet never slows down off of odd collisions
        sfx.pitch = Random.Range(1f, 2f);
        sfx.PlayOneShot(sfx.clip);
    }
}
