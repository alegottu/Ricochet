using UnityEngine;

public abstract class MediaController<T> : MonoBehaviour
{
    [SerializeField] protected T host = default;
    [SerializeField] protected Animator anim = null;
    [SerializeField] protected AudioSource sfx = null;
    [SerializeField] protected SpriteRenderer sprite = null;
    [SerializeField] protected Health health = null;

    protected virtual void Awake()
    {
        health.OnDeath += OnDeathEventHandler;
        health.OnDamageTaken += OnDamageTakenEventHandler;
    }

    protected abstract void OnDeathEventHandler();
    protected abstract void OnDamageTakenEventHandler();

    protected virtual void OnDisable()
    {
        health.OnDeath -= OnDeathEventHandler;
        health.OnDamageTaken -= OnDamageTakenEventHandler;
    }
}
