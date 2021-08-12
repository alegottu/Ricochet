using UnityEngine;
using System;

public class Health : MonoBehaviour
{
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
    }

    public void Heal(int amount)
    {
        _health += amount;
        _health = _health > maxHealth ? maxHealth : _health; 
    }

    public void Die()
    {
        OnDeath?.Invoke();
        Destroy(this);
    } 
}
