using UnityEngine;

public class Jolt : PlayerEffect
{
    [SerializeField] private float rechargeDecreaseAmount = 0;

    public override void CastEffect(Player player)
    {
        player.SetRechargeDecrease(rechargeDecreaseAmount);
    }
}
