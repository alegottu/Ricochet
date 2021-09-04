using UnityEngine;

public class Ammo : PlayerEffect
{
    public override void CastEffect(Player player)
    {
        player.AddExtraBullet();
    }
}
