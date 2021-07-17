using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiOreController : MonoBehaviour
{

    public GameObject remains;
    public PlayerStats playerStats;
    
    public void Shatter()
    {
        Instantiate(remains, transform.position, transform.rotation);
        playerStats.IncreaseHealth(10);
        Destroy(gameObject);
    }

}
