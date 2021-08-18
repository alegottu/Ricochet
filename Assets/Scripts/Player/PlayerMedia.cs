using UnityEngine;

public class PlayerMedia : MediaController<Player>
{
    [SerializeField] private Animator specialMeter = null;
    [SerializeField] private Animator healthbar = null;

    protected override void OnEnable()
    {
        base.OnEnable();

        host.OnChargeGained += OnChargeGainedEventHandler;
        host.OnRadiationDamage += OnRadiationDamageEventHandler;
        Bullet.OnBulletDestroyed += OnBulletDestroyedEventHandler;
        Enemy.OnEnemyDestroyed += OnEnemyDestroyedEventHandler;
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

    protected override void OnDeathEventHandler()
    {
        anim.SetTrigger("Kill");
    }

    protected override void OnDamageTakenEventHandler()
    {
        healthbar.SetTrigger("Damage");
    }

    private void OnRadiationDamageEventHandler()
    {
        anim.SetTrigger("Radiation");
    }

    private void OnBulletDestroyedEventHandler()
    {
        anim.SetTrigger("Ammo");
    }

    private void OnEnemyDestroyedEventHandler()
    {
        anim.SetTrigger("Damage");
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        host.OnChargeGained -= OnChargeGainedEventHandler;
        host.OnRadiationDamage -= OnRadiationDamageEventHandler;
        Bullet.OnBulletDestroyed -= OnBulletDestroyedEventHandler;
        Enemy.OnEnemyDestroyed -= OnEnemyDestroyedEventHandler;
    }
}