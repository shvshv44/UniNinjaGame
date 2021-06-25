using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaHealthController : MonoBehaviour
{

    public GameObject [] healths;

    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healths.Length;
    }

    public void TakeDamage()
    {
        if(currentHealth != 0)
        {
            currentHealth--;
            Destroy(healths[currentHealth]);
        }
    }
}
