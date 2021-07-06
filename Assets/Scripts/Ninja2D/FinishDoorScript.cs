using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishDoorScript : MonoBehaviour
{

    public int openSpeed = 1;
    public float animationTime = 3;
    private bool isOpened = false;

    void Update()
    {
        if(isOpened)
        {
            StartCoroutine(OpenAnimation());
        }
    }

    public void Open()
    {
        isOpened = true;
    }

    private IEnumerator OpenAnimation()
    {
        transform.position = transform.position + new Vector3(0, -openSpeed * Time.deltaTime, 0);
        yield return new WaitForSecondsRealtime(animationTime);
        Destroy(gameObject);
    }
}
