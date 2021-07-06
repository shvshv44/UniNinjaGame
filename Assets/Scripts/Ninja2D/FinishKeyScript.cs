using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishKeyScript : MonoBehaviour
{

    public FinishDoorScript finishDoor;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            finishDoor.Open();
            Destroy(gameObject);
        }
    }
}
