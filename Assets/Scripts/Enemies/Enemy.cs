using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public static event Action OnEnemyDestroyed; // Destroyed, not killed

    [SerializeField] protected Health health;
    [SerializeField] protected Vector2 speed = Vector2.zero;
    [SerializeField] protected float speedFluctation = 1;

    private Health player = null;

    protected virtual void OnEnable()
    {
        health.OnDeath += OnDeathEventHandler;
    }

    private void OnDeathEventHandler()
    {
        Destroy(this);
    }

    public void SetUp(Health player, float speedMultiplier)
    {
        this.player = player;
        speed = speed * speedMultiplier;
        speed *= new Vector2(UnityEngine.Random.Range(1f, speedFluctation),
            UnityEngine.Random.Range(1f, speedFluctation));
    }

    public void Destroy()
    {
        player.TakeDamage(1);
        OnEnemyDestroyed?.Invoke();

        Destroy(gameObject);
    }

    public void Kill()
    {
        health.TakeDamage(health.maxHealth);
    }

    public void Turn()
    {
        speed *= new Vector2(-1, 1);
    }

    protected virtual void Move()
    {
        transform.position -= (Vector3)speed * Time.deltaTime;
    }

    protected virtual void FixedUpdate()
    {
        Move();
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