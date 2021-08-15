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
        anim.SetTrigger("Damage");
    }

    // For use as an animator event
    private void OnDeathComplete()
    {
        Destroy(gameObject);
    }
}
