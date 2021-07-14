using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    public Consumable consumable;
    public Inventory inventory;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            inventory.AddItem(consumable);
            Destroy(gameObject);
        }
    }

}
