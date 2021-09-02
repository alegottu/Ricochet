using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public event Action OnDamageTaken;
    public event Action OnHeal;
    public event Action OnDeath;

    private int _health = 1;
    public int health { get { return _health; } }
    [SerializeField] private int _maxHealth = 1;
    public int maxHealth { get { return _maxHealth; } }

    private void Awake()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;

        if (_health <= 0)
        {
            Die();
        }

        OnDamageTaken?.Invoke();
    }

    public void Heal(int amount)
    {
        _health = Mathf.Min(_maxHealth, _health + amount);
        OnHeal?.Invoke();
    }

    public void Die()
    {
        OnDeath?.Invoke();
        Destroy(this);
    } 
}
