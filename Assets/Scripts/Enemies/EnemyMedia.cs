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

    // For use as an animator event
    private void OnDeathComplete()
    {
        Destroy(gameObject);
    }
}
