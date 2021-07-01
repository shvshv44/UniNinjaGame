using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NinjaHealthController : MonoBehaviour
{

    public GameObject [] healths;
    public float maxXRecoil = 0;
    public float maxYRecoil = 0;
    public float minXRecoil = 0;
    public float minYRecoil = 0;
    public float recoveryTime = 3;
    public Color hurtColor;
    


    private int currentHealth;
    private bool isResistable;
    private float currentRecoveryTime;
    private NinjaController controller;
    private Renderer rend;
    private Color baseColor;

    private Rigidbody2D RBody { get; set; }

    void Start()
    {
        currentRecoveryTime = 0;
        currentHealth = healths.Length;
        isResistable = false;
        RBody = GetComponent<Rigidbody2D>();
        controller = GetComponent<NinjaController>();
        rend = GetComponent<Renderer>();
        baseColor = rend.material.color;

    }

    private void Update()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
            if (currentRecoveryTime <= 0)
            {
                isResistable = false;
                currentRecoveryTime = 0;
            }
        }

        if (isResistable)
        {
            rend.material.color = hurtColor;
        } else
        {
            rend.material.color = baseColor;
        }
    }

    public void TakeDamage()
    {
        if(currentHealth != 0 && !isResistable)
        {
            currentHealth--;
            Destroy(healths[currentHealth]);
            currentRecoveryTime = recoveryTime;
            isResistable = true;
            RecoilPlayer();
            HandleDeath();
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

    private void HandleDeath()
    {
        if (currentHealth == 0)
        {
            SceneManager.LoadScene("2DPlatformLose", LoadSceneMode.Single);
        }
    }
}
