using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeCarScript : MonoBehaviour
{
    public Rigidbody rb;
    public Transform car;
    public float speed = 17;
    public float jumpHeight = 7f;
    public bool isGrounded;
    public float NumberJumps = 0f;
    public float MaxJumps = 2;
    AudioSource jumpSound;

    Vector3 rotationRight = new Vector3(0, 70, 0);
    Vector3 rotationLeft = new Vector3(0, -70, 0);

    Vector3 forward = new Vector3(-1, 0, 0);
    Vector3 backward = new Vector3(1, 0, 0);

    private bool isCanDrive;

    void Start()
    {
        isCanDrive = false;
        jumpSound = GetComponent<AudioSource>();
    }

    IEnumerator WaitInStartSeconds()
    {
        while (!isCanDrive)
        {
            yield return new WaitForSeconds(2);
            isCanDrive = true;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        isGrounded = true;
        NumberJumps = 0;
    }

    void FixedUpdate()
    {
        StartCoroutine(WaitInStartSeconds());

        if(isCanDrive)
        {
            transform.Translate(new Vector3(-6, 0, 0) * Time.fixedDeltaTime);

            if (Input.GetKey("s"))
            {
                transform.Translate(backward * speed * Time.fixedDeltaTime);
            }
            if (Input.GetKey("w"))
            {
                transform.Translate(forward * speed * Time.fixedDeltaTime);
            }

            if (Input.GetKey("d"))

            {
                Quaternion deltaRotationRight = Quaternion.Euler(rotationRight * Time.fixedDeltaTime);
                rb.MoveRotation(rb.rotation * deltaRotationRight);
            }

            if (Input.GetKey("a"))
            {
                Quaternion deltaRotationLeft = Quaternion.Euler(rotationLeft * Time.fixedDeltaTime);
                rb.MoveRotation(rb.rotation * deltaRotationLeft);
            }

            if (NumberJumps > MaxJumps - 1)
            {
                isGrounded = false;
            }

            if (isGrounded)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    jumpSound.Play();
                    if (NumberJumps == 0)
                    {
                        rb.velocity = new Vector3(0, 5, 0);
                    }
                    else
                    {
                        rb.velocity = new Vector3(0, 8, 0);
                    }
                    NumberJumps += 1;
                }
            }
        }
       
    }
}
 