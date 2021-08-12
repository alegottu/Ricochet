using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private PlayerData data = null;
    [SerializeField] private Rigidbody2D rb = null;

    private Health player = null;

    public void SetUp(Health player)
    {
        transform.eulerAngles = Vector3.forward * Random.Range(data.bulletAngleRange.x, data.bulletAngleRange.y);
        rb.velocity = transform.right * data.bulletSpeed;

        this.player = player;
    }

    public void Reflect(Vector3 surfaceNormal)
    {
        rb.velocity = Vector3.Reflect(rb.velocity, surfaceNormal);
    }

    private void OnDestroy()
    {
        player.TakeDamage(1);
    }
}
