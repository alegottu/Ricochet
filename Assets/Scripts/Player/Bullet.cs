using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static event Action OnBulletDestroyed;

    [SerializeField] protected PlayerData data = null;
    [SerializeField] protected Rigidbody2D rb = null;

    private Health player = null;

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

    public void Setup(Health player)
    {
        transform.eulerAngles = Vector3.forward * UnityEngine.Random.Range(data.bulletAngleRange.x, data.bulletAngleRange.y);
        rb.velocity = transform.right * data.bulletSpeed;

        this.player = player;
    }

    private void OnCollisionExit2D(Collision2D _)
    {
        rb.velocity = rb.velocity.normalized * data.bulletSpeed; // To ensure the bullet never slows down off of odd collisions
    }

    protected virtual void OnDestroy()
    {
        OnBulletDestroyed?.Invoke();
        player.TakeDamage(1);
    }

    private void OnDisable()
    {
        Wall.OnWallAttack -= OnWallAttackEventHandler;
    }
}
