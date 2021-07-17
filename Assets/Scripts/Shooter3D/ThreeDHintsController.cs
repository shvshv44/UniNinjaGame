using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreeDHintsController : MonoBehaviour
{

    public Text hintTxt;

    public void ChangeText(string txt)
    {
        hintTxt.text = txt;
    }

}
