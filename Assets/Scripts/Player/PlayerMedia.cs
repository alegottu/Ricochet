using UnityEngine;

public class PlayerMedia : MediaController<Player>
{
    [SerializeField] private Animator specialMeter = null;

    protected override void Awake()
    {
        base.Awake();

        host.OnChargeGained += OnChargeGainedEventHandler;
        host.OnRadiationDamage += OnRadiationDamageEventHandler;
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

    protected override void OnDamageTakenEventHandler()
    {
        anim.SetTrigger("Damage");
    }
}