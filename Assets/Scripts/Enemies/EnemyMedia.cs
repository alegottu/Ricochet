using UnityEngine;

public class EnemyMedia : MediaController<Enemy>
{
    public void SetTrigger(string name)
    {
        anim.SetTrigger(name);
    }

    protected override void OnDeathEventHandler()
    {
        anim.SetTrigger("Kill");
    }

    protected override void OnDamageTakenEventHandler()
    {
        return; // add separate damage animation that can not be triggered after death is triggered
    }

    // For use as an animator event
    private void OnDeathComplete()
    {
        Destroy(gameObject);
    }
}
