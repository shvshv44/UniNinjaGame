using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    void Awake()
    {
       // transform.Rotate(50.0f, -30.0f, 0.0f, Space.World);
    }

    void Update()
    {
        //transform.Rotate(0, 1 * Time.deltaTime, 0, Space.World);
        transform.Rotate(-1 * Time.deltaTime, 1 * Time.deltaTime, 0, Space.Self);
    }
}
