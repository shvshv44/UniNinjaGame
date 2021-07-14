using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Consumable : MonoBehaviour
{

    public string name;
    [TextArea(1,6)]
    public string description;
    public float intensity;
    public Sprite icon;

    public abstract void Consume(PlayerStats stats);

}
