using System.Collections;
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

    public void PlayRandomSound(int sound, float minPitch=1f, float maxPitch=3f)
    {
        sfx.pitch = Random.Range(minPitch, maxPitch);
        sfx.PlayOneShot(sounds[sound]);
        StartCoroutine(FinishRandomSound());
    }

    private IEnumerator FinishRandomSound()
    {
        yield return new WaitWhile(() => sfx.isPlaying);
        sfx.pitch = 1;
    }

    protected abstract void OnDeathEventHandler();
    protected abstract void OnDamageTakenEventHandler();

    protected virtual void OnDisable()
    {
        health.OnDeath -= OnDeathEventHandler;
        health.OnDamageTaken -= OnDamageTakenEventHandler;
    }
}
