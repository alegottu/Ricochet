using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static event Action OnBulletDestroyed;

    [SerializeField] private PlayerData data = null;
    [SerializeField] private Rigidbody2D rb = null;

    private Health player = null;

    private void Awake()
    {
        Wall.OnWallAttack += OnWallAttackEventHandler;
    }

    private void OnWallAttackEventHandler(float attackTime)
    {
        
    }

    private IEnumerator SlowDown(float attackTime)
    {
        yield return new WaitForEndOfFrame();
    }

    public void SetUp(Health player)
    {
        transform.eulerAngles = Vector3.forward * UnityEngine.Random.Range(data.bulletAngleRange.x, data.bulletAngleRange.y);
        rb.velocity = transform.right * data.bulletSpeed;

        this.player = player;
    }

    public void Reflect(Vector3 surfaceNormal)
    {
        rb.velocity = Vector3.Reflect(rb.velocity, surfaceNormal);
    }

    private void OnDestroy()
    {
        OnBulletDestroyed?.Invoke();
        player.TakeDamage(1);

        Wall.OnWallAttack -= OnWallAttackEventHandler;
    }
}
