using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenThrower : MonoBehaviour
{

    public Camera myCamera;
    public GameObject shuriken;
    public float shurikenSpeed = 0;
    public float throwingCooldown = 0;
    public PlayerStats playerStats;

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
            GameObject shurikenThrown = Instantiate(shuriken, transform.position + (ray.direction * 3), transform.rotation);
            //GameObject shurikenThrown = Instantiate(shuriken, transform.position, transform.rotation);
            shurikenThrown.GetComponent<Rigidbody>().velocity = ray.direction * shurikenSpeed;
            shurikenThrown.GetComponent<Shuriken3DController>().damage = playerStats.GetDamage();
            shurikenThrown.tag = "Shuriken";
            currentThrowingCooldown = throwingCooldown;
        }

        if(currentThrowingCooldown > 0)
        {
            currentThrowingCooldown -= Time.deltaTime;
            if (currentThrowingCooldown < 0)
                currentThrowingCooldown = 0;
        }

        if (Input.GetMouseButtonDown(1))
        {
            float x = Screen.width / 2;
            float y = Screen.height / 2;
            //float y = Screen.height * 4 / 6;

            int layerMask = 1 << 8;
            //layerMask = ~layerMask;
            Ray ray = myCamera.ScreenPointToRay(new Vector3(x, y, 0));
            RaycastHit hit;
            Vector3 lookAtPosition = myCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, myCamera.nearClipPlane));

            if (Physics.Raycast(lookAtPosition, ray.direction, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.gameObject.TryGetComponent(out VoronoiOreController voc))
                {
                    voc.Shatter();
                }

            }

            Debug.DrawLine(lookAtPosition, transform.position + (ray.direction * 10f), Color.green);
        }
    }
}
