using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : Consumable
{
    public override void Consume(PlayerStats stats)
    {
        stats.DefineSpeedBoost(intensity);
    }
}
