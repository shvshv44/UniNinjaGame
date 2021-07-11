using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Consumable
{
    public override void Consume(PlayerStats stats)
    {
        stats.IncreaseHealth((int) intensity);
    }
}
