using System;
using UnityEngine;

public abstract class PlayerEffect : TemporaryObject
{
    public static event Action<PlayerEffect> OnPlayerStatChange;

    [SerializeField] private float lifetime = 0;
    [SerializeField] private AudioSource sfx = null;

    private void Awake()
    {
        StartCoroutine(Timer(lifetime));
    }

    public abstract void CastEffect(Player player);

    private void OnTriggerEnter2D(Collider2D collider)
    {
        sfx.PlayOneShot(sfx.clip);
        OnPlayerStatChange?.Invoke(this);
        Destroy(gameObject);
    }
}