using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTriggerScript : MonoBehaviour
{

    public HintController hint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            hint.Activate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hint.DeActivate();
        }  
    }
}
