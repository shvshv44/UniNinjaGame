using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : MonoBehaviour
{

    public float speed;

    private string [] freinlyTags = new string[2] { "Player", "Hint" };

    private void FixedUpdate()
    {
        Vector3 vecToAdd = new Vector3(speed * Time.deltaTime, 0, 0);
        transform.position += vecToAdd;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (! Array.Exists(freinlyTags, element => element.Equals(collision.tag)))
        {
            Destroy(gameObject);
        }
    }
}
