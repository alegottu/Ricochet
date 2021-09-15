using UnityEngine;

public class EnemyMedia : MediaController<Enemy>
{
    [SerializeField] private float explosionChance = 0;

    protected override void OnDeathEventHandler()
    {
        if (Random.Range(0f, 1f) <= explosionChance)
        {
            CameraController.Instance.StartShake(1.5f);
            PlayEvent("Explode", 2);
        }
        else
        {
            CameraController.Instance.StartShake(1);
            PlayEvent("Kill", 1);
        }
    }

    protected override void OnDamageTakenEventHandler()
    {
        CameraController.Instance.StartShake(0.25f);
        PlayEvent("Damage", 0);
    }

    // For use as an animator event
    private void OnDeathComplete()
    {
        Destroy(gameObject);
    }
}
