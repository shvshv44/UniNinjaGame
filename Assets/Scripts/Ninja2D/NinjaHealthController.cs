using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaHealthController : MonoBehaviour
{

    public GameObject [] healths;
    public float maxXRecoil = 0;
    public float maxYRecoil = 0;
    public float minXRecoil = 0;
    public float minYRecoil = 0;
    public float recoveryTime = 1; // TODO: make it work!!

    private int currentHealth;
    private bool isResistable;
    private NinjaController controller;

    private Rigidbody2D RBody { get; set; }

    void Start()
    {
        currentHealth = healths.Length;
        isResistable = false;
        RBody = GetComponent<Rigidbody2D>();
        controller = GetComponent<NinjaController>();
    }

    public void TakeDamage()
    {
        if(currentHealth != 0 && !isResistable)
        {
            currentHealth--;
            Destroy(healths[currentHealth]);
            RecoilPlayer();
        }
    }

    private void RecoilPlayer()
    {
        float xForce = Random.Range(minXRecoil, maxXRecoil);
        float yForce = Random.Range(minYRecoil, maxYRecoil);

        // Define direction
        if (Random.Range(0, 1) > 0.5)
            xForce *= -1;

        Vector2 force = new Vector2(xForce, yForce);
        controller.DefineHitForce(force);
    }
}
