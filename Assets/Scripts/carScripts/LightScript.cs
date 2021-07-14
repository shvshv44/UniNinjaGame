using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    void Start()
    {
    
    }

    void Update()
    {
        transform.Rotate(0, 1 * Time.deltaTime, 0, Space.World);
        transform.Rotate(-1 * Time.deltaTime, 0, 0, Space.Self);
    }
}
