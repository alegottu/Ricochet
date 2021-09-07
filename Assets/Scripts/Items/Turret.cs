using UnityEngine;

public class Turret : PlayerEffect
{
    public override void CastEffect(Player player)
    {
        player.ShootExtraBullet();
    }
}
