using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public static event Action OnEnemyDestroyed; // Destroyed, not killed

    [SerializeField] protected Rigidbody2D rb = null;
    [SerializeField] protected Health health = null;
    [SerializeField] protected EnemyData data = null;

    private Health player = null;

    protected virtual void OnEnable()
    {
        health.OnDeath += OnDeathEventHandler;
    }

    private void OnDeathEventHandler()
    {
        Destroy(this);
    }

    public virtual void SetUp(Health player, float speedMultiplier)
    {
        this.player = player;

        rb.velocity = data.speed * new Vector2(data.direction.x * UnityEngine.Random.Range(0f, 1f), data.direction.y);
        rb.velocity *= UnityEngine.Random.Range(1f, data.speedFluctation);

        if (UnityEngine.Random.Range(-1, 1) == -1)
        {
            Reflect();
        }
    }

    public void Destroy()
    {
        player.TakeDamage(1);
        OnEnemyDestroyed?.Invoke();

        Destroy(gameObject);
    }

    public void Reflect()
    {
        rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        health.TakeDamage(1);
    }

    protected virtual void OnDisable()
    {
        health.OnDeath -= OnDeathEventHandler;
    }
}