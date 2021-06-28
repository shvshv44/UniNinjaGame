using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightWatchController : MonoBehaviour
{

    public KnightController controller;
    public Transform leftEdge;
    public Transform rightEdge;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.transform.position.x < rightEdge.position.x && collision.transform.position.x > leftEdge.position.x)
                controller.SetAggressiveMode(collision.gameObject);
            else
                controller.SetNormalMode();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            controller.SetNormalMode();
    }
}
