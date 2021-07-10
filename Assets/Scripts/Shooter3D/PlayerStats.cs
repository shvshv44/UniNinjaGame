using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    public int maxHealth;
    public Text healthTxt;

    private int currentHelath;

    void Start()
    {
        currentHelath = maxHealth;
    }


}
