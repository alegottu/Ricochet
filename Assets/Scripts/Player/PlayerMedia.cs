using UnityEngine.UI;
using UnityEngine;

public class PlayerMedia : MediaController<Player>
{
    [SerializeField] private PlayerData data = null; // Potentially change to just a single float for performance, only used for radThreshold
    [SerializeField] private Animator specialMeter = null;
    [SerializeField] private Animator healthbar = null;
    [SerializeField] private Slider photonMeter = null; 

    protected override void OnEnable()
    {
        base.OnEnable();

        host.OnChargeGained += OnChargeGainedEventHandler;
        host.OnOverheatDamage += OnOverheatDamageEventHandler;
        host.OnPhotonLoss += OnPhotonLossEventHandler;
        Bullet.OnBulletDestroyed += OnBulletDestroyedEventHandler;
        Enemy.OnEnemyDestroyed += OnEnemyDestroyedEventHandler;
        health.OnHeal += OnHealEventHandler;
    }

    private void OnPhotonLossEventHandler(float photonsPercent)
    {
        photonMeter.value = photonsPercent / data.photonMax;
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

    private void OnOverheatDamageEventHandler()
    {
        anim.SetTrigger("Overheat");
    }

    private void OnBulletDestroyedEventHandler()
    {
        anim.SetTrigger("Ammo");
    }

    private void OnEnemyDestroyedEventHandler()
    {
        anim.SetTrigger("Damage");
    }

    private void OnHealEventHandler()
    {
        healthbar.SetTrigger("Heal");
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        host.OnChargeGained -= OnChargeGainedEventHandler;
        host.OnOverheatDamage -= OnOverheatDamageEventHandler;
        host.OnPhotonLoss -= OnPhotonLossEventHandler;
        Bullet.OnBulletDestroyed -= OnBulletDestroyedEventHandler;
        Enemy.OnEnemyDestroyed -= OnEnemyDestroyedEventHandler;
        health.OnHeal -= OnHealEventHandler;
    }
}