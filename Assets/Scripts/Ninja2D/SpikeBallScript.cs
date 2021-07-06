using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBallScript : MonoBehaviour
{

    public Transform[] locations;
    public float moveSpeed;
    public float waitingSec;

    private int destPoint;
    private Transform selfPosition;

    private void Start()
    {
        selfPosition = GetComponent<Transform>();
        destPoint = 0;
        selfPosition.position = locations[destPoint].position;
    }

    private void FixedUpdate()
    {
        if (transform.position == locations[destPoint].position)
        {
            StartCoroutine(WaitForNextLocation());
            destPoint++;
        }

        if (destPoint >= locations.Length)
        {
            destPoint = 0;
        }

        selfPosition.position = Vector2.MoveTowards(transform.position, locations[destPoint].position, moveSpeed * Time.deltaTime);
    }

    private IEnumerator WaitForNextLocation()
    {
        yield return new WaitForSeconds(waitingSec);
    }
}
