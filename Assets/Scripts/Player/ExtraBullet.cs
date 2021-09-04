using System;
using UnityEngine;

public class ExtraBullet : Projectile
{
    public static event Action OnExtraBulletDestroyed;
    
    private void Awake()
    {
        SetVelocity(UnityEngine.Random.Range(data.bulletAngleRange.x, data.bulletAngleRange.y));
    }

    private void OnDestroy()
    {
        OnExtraBulletDestroyed?.Invoke();
    }
}
