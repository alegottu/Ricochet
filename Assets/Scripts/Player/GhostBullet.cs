using UnityEngine;

public class GhostBullet : Projectile
{
    public void Setup(int angle)
    {
        SetVelocity(angle);
    }
}
