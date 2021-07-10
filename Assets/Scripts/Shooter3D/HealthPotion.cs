using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Consumable
{
    public override void Consume()
    {
        Debug.Log("Consume Potion!");
    }
}
