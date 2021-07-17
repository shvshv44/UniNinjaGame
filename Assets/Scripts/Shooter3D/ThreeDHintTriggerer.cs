using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreeDHintTriggerer : MonoBehaviour
{

    [TextArea(10,10)]
    public string text;
    public ThreeDHintsController hintsController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            hintsController.ChangeText(text);
            Destroy(gameObject);
        }
    }

}
