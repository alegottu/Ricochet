using UnityEngine;

public abstract class Item : TemporaryObject
{
    [SerializeField] protected float lifetime = 0;
    [SerializeField] private AudioSource sfx = null;

    protected virtual void Awake()
    {
        StartCoroutine(Timer(lifetime));
    }

    protected abstract void CastEffect();

    protected virtual void OnTriggerEnter2D(Collider2D collider) // Should not be able to collide with enemy layer (using the collision matrix)
    {
        sfx.PlayOneShot(sfx.clip);
        CastEffect();
        Destroy(gameObject);
    }
}
