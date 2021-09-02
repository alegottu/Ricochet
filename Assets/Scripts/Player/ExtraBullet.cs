using System;
using UnityEngine;

public class ExtraBullet : Projectile
{
    public static event Action<int> OnExtraBulletDestroyed;
    
    private static int amount = 0; 
    private int index = amount;// Used to track which bullet should be replaced when destroyed 

    public static void IncreaseBulletCount()
    {
        amount++;
    }

    private void Awake()
    {
        SetVelocity(UnityEngine.Random.Range(data.bulletAngleRange.x, data.bulletAngleRange.y));
    }

    private void OnDestroy()
    {
        OnExtraBulletDestroyed?.Invoke(index);
    }
}
