using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken3DController : MonoBehaviour
{

    public int damage = 1;
    public AudioSource audioSrc;
    public AudioClip shurikenHitSound;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            audioSrc.PlayOneShot(shurikenHitSound, 0.7f);
            Destroy(gameObject);
        }
    }


}
