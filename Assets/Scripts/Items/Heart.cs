﻿using UnityEngine;

public class Heart : PlayerEffect
{
    public override void CastEffect(Player player)
    {
        player.GetComponent<Health>().Heal(1);
    }
}
