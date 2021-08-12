using UnityEngine;

public class PlayerMedia : MediaController<Player>
{
    [SerializeField] private Animator specialMeter = null;

    protected override void Awake()
    {
        base.Awake();

        host.OnChargeGained += OnChargeGainedEventHandler;
        host.OnRadiationDamage += OnRadiationDamageEventHandler;
        Bullet.OnBulletDestroyed += OnBulletDestroyedEventHandler;
        Enemy.OnEnemyDestroyed += OnEnemyDestroyedEventHandler;

        // todo: add 3 dots for health UI and have it change with OnDamageTaken
    }

    private void OnChargeGainedEventHandler(bool gain)
    {
        if (gain)
        {
            specialMeter.SetTrigger("Fill");
        }
        else
        {
            specialMeter.SetTrigger("Deplete");
        }
    }

    private void OnRadiationDamageEventHandler()
    {
        anim.SetTrigger("Radiation");
    }

    protected override void OnDeathEventHandler()
    {
        anim.SetTrigger("Kill");
    }

    private void OnBulletDestroyedEventHandler()
    {
        anim.SetTrigger("Ammo");
    }

    private void OnEnemyDestroyedEventHandler()
    {
        anim.SetTrigger("Damage");
    }
}