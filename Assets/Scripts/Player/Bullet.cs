using System;
using System.Collections;
using UnityEngine;

public class Bullet : Projectile
{
    public static event Action OnBulletDestroyed;

    private void Awake()
    {
        SetVelocity(UnityEngine.Random.Range(data.bulletAngleRange.x, data.bulletAngleRange.y));
    }

    private void OnEnable()
    {
        Wall.OnWallAttack += OnWallAttackEventHandler;
    }

    private void OnWallAttackEventHandler(float attackTime)
    {
        StartCoroutine(SlowDown(attackTime));
    }

    private IEnumerator SlowDown(float attackTime)
    {
        Vector2 originalSpeed = rb.velocity;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(attackTime);

        rb.velocity = originalSpeed;
    }

    protected virtual void OnDestroy()
    {
        OnBulletDestroyed?.Invoke();
    }

    private void OnDisable()
    {
        Wall.OnWallAttack -= OnWallAttackEventHandler;
    }
}
