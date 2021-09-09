using UnityEngine;

public abstract class MediaController<T> : MonoBehaviour
{
    [SerializeField] protected T host = default;
    [SerializeField] protected Animator anim = null;
    [SerializeField] protected AudioSource sfx = null;
    [SerializeField] protected AudioClip[] sounds = null;
    [SerializeField] protected SpriteRenderer sprite = null;
    [SerializeField] protected Health health = null;

    protected virtual void OnEnable()
    {
        health.OnDeath += OnDeathEventHandler;
        health.OnDamageTaken += OnDamageTakenEventHandler;
    }

    public void PlayEvent(string trigger, int sound)
    {
        anim.SetTrigger(trigger);
        sfx.PlayOneShot(sounds[sound]);
    }

    public void PlaySound(int sound)
    {
        sfx.PlayOneShot(sounds[sound]);
    }

    public void PlayRandomSound(int sound)
    {
        float originalPitch = sfx.pitch;
        sfx.pitch = Random.Range(-3f, 3f);
        sfx.PlayOneShot(sounds[sound]);
        sfx.pitch = originalPitch;
    }

    protected abstract void OnDeathEventHandler();
    protected abstract void OnDamageTakenEventHandler();

    protected virtual void OnDisable()
    {
        health.OnDeath -= OnDeathEventHandler;
        health.OnDamageTaken -= OnDamageTakenEventHandler;
    }
}
