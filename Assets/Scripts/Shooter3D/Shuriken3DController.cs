using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken3DController : MonoBehaviour
{

    public int damage = 1;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }


}
