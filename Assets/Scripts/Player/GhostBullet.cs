using UnityEngine;

public class GhostBullet : Bullet
{
    public void Setup(int angle)
    {
        transform.eulerAngles = Vector3.forward * angle;
        rb.velocity = transform.right * data.bulletSpeed;
    }

    protected override void OnDestroy()
    {
        return;
    }
}
