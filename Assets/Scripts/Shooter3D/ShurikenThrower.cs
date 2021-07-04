using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenThrower : MonoBehaviour
{

    public Camera myCamera;
    public Rigidbody shuriken;
    public float shurikenSpeed = 0;
    public float throwingCooldown = 0;

    private float currentThrowingCooldown;

    void Start()
    {
        currentThrowingCooldown = 0;
    }

    void Update()
    {
        if (currentThrowingCooldown == 0 && Input.GetMouseButtonDown(0))
        {
            float x = Screen.width / 2;
            //float y = Screen.height / 2;
            float y = Screen.height * 4 / 6;
            Ray ray = myCamera.ScreenPointToRay(new Vector3(x, y, 0));
            Rigidbody grenade = Instantiate(shuriken, transform.position, transform.rotation);
            grenade.velocity = ray.direction * shurikenSpeed;
            currentThrowingCooldown = throwingCooldown;
        }

        if(currentThrowingCooldown > 0)
        {
            currentThrowingCooldown -= Time.deltaTime;
            if (currentThrowingCooldown < 0)
                currentThrowingCooldown = 0;
        }
    }
}
