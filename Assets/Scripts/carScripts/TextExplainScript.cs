using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextExplainScript : MonoBehaviour
{
    public Text text;
    IEnumerator ShowMessageCoroutine(string message, float timeToShow = 10)
    {
        text.enabled = true;
        text.text = "3";
        yield return new WaitForSecondsRealtime(0.5f);
        text.text = "2";
        yield return new WaitForSecondsRealtime(0.5f);
        text.text = "1";
        yield return new WaitForSecondsRealtime(0.5f);

        text.text = "Start";
        yield return new WaitForSecondsRealtime(1f);

        text.text = "U have 3 lives";
        yield return new WaitForSecondsRealtime(1.5f);

        text.text = "Use space to jump";
        yield return new WaitForSecondsRealtime(2f);

        text.text = "Jump twice";
        yield return new WaitForSecondsRealtime(2f);

        text.text = "A for turn right";
        yield return new WaitForSecondsRealtime(2f);

        text.text = "D for turn left";
        yield return new WaitForSecondsRealtime(2f);

        text.text = "";
        yield return new WaitForSecondsRealtime(1f);

        text.text = "Turn Left";
        yield return new WaitForSecondsRealtime(3f);

        // Hide the text
        text.enabled = false;
        text.text = "";
    }

    void Start()
    {
        StartCoroutine(ShowMessageCoroutine("Hello", 8));
    }
}
