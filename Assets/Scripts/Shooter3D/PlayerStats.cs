using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    public int maxHealth;
    public Text healthTxt;
    public Text damageTxt;
    public Text speedTxt;
    public int baseDamage;
    public float baseSpeed;

    private int currentHealth;
    private float speedMultiplier;
    private int damageMultiplier;

    void Start()
    {
        currentHealth = maxHealth;
        speedMultiplier = 1f;
        damageMultiplier = 1;
        UpdateUI();
    }

    public void IncreaseHealth(int health)
    {
        currentHealth += health;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateHealthText();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth < 0)
        {
            currentHealth = 0;
        }

        if(currentHealth == 0)
        {
            Die();
        }

        UpdateHealthText();
    }

    public float GetSpeed()
    {
        return baseSpeed * speedMultiplier;
    }

    public int GetDamage()
    {
        return baseDamage * damageMultiplier;
    }

    public void DefineSpeedBoost(float speedBoost)
    {
        if (speedMultiplier == 1)
        {
            speedMultiplier = speedBoost;
        }

        UpdateSpeedText();
    }

    public void DefineDamageBoost(int damageBoost)
    {
        if (damageMultiplier == 1)
        {
            damageMultiplier = damageBoost;
        }

        UpdateDamageText();
    }

    public void StopSpeedBoosting()
    {
        speedMultiplier = 1f;
        UpdateSpeedText();
    }

    public void StopDamageBoosting()
    {
        damageMultiplier = 1;
        UpdateDamageText();
    }

    private void Die()
    {
        Debug.Log("YOU HAVE DIED!!!");
    }

    private void UpdateHealthText()
    {
        healthTxt.text = currentHealth + "/" + maxHealth;
    }

    private void UpdateDamageText()
    {
        damageTxt.text = "Damage: " + GetDamage();
    }

    private void UpdateSpeedText()
    {
        speedTxt.text = "Speed: " + GetSpeed();
    }

    public void UpdateUI()
    {
        UpdateHealthText();
        UpdateDamageText();
        UpdateSpeedText();
    }


}
