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

        health.OnHeal += OnHealEventHandler;
    }

    public void UpdatePhotonMeter(float photonsPercent)
    {
        photonMeter.value = photonsPercent / data.photonMax;
    }

    public void UpdateSpecialMeter(string trigger)
    {
        specialMeter.SetTrigger(trigger);
    }

    protected override void OnDeathEventHandler()
    {
        anim.SetTrigger("Kill");
    }

    protected override void OnDamageTakenEventHandler()
    {
        healthbar.SetTrigger("Damage");
    }

    public void PlayWarning(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    private void OnHealEventHandler()
    {
        healthbar.SetTrigger("Heal");
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        health.OnHeal -= OnHealEventHandler;
    }
}