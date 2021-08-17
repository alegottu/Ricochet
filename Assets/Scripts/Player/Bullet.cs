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
        StartCoroutine(SlowDown(attackTime));
    }

    private IEnumerator SlowDown(float attackTime)
    {
        Vector2 originalSpeed = rb.velocity;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(attackTime);

        rb.velocity = originalSpeed;
    }

    public void SetUp(Health player)
    {
        transform.eulerAngles = Vector3.forward * UnityEngine.Random.Range(data.bulletAngleRange.x, data.bulletAngleRange.y);
        rb.velocity = transform.right * data.bulletSpeed;

        this.player = player;
    }

    private void OnCollisionExit2D(Collision2D _)
    {
        rb.velocity = rb.velocity.normalized * data.bulletSpeed; // To ensure the bullet never slows down off of odd collisions
    }

    private void OnDestroy()
    {
        OnBulletDestroyed?.Invoke();
        player.TakeDamage(1);

        Wall.OnWallAttack -= OnWallAttackEventHandler;
    }
}
