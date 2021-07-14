using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePotion : Consumable
{
    public override void Consume(PlayerStats stats)
    {
        stats.DefineDamageBoost((int) intensity);
    }
}
