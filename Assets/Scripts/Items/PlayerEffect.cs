using System;
using UnityEngine;

public abstract class PlayerEffect : TemporaryObject
{
    public static event Action<PlayerEffect> OnPlayerStatChange;

    [SerializeField] private float lifetime = 0;

    private void Awake()
    {
        StartCoroutine(Timer(lifetime));
    }

    public abstract void CastEffect(Player player);

    private void OnTriggerEnter2D(Collider2D collider)
    {        
        OnPlayerStatChange?.Invoke(this);
    }
}