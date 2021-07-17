using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private float currentDamageCooldown;
    private float currentSpeedCooldown;

    private const float COOLDOWN = 30f;
    private const string SPEED_PREFIX = "Speed: ";
    private const string DAMAGE_PREFIX = "Damage: ";

    void Start()
    {
        currentDamageCooldown = 0;
        currentDamageCooldown = 0;
        currentHealth = maxHealth;
        speedMultiplier = 1f;
        damageMultiplier = 1;
        UpdateUI();
    }

    void Update()
    {
        UpdateSpeedCooldown();
        UpdateDamageCooldown();
    }

    private void UpdateSpeedCooldown()
    {
        if(currentSpeedCooldown > 0)
        {
            currentSpeedCooldown -= Time.deltaTime;

            if(currentSpeedCooldown <= 0)
            {
                currentSpeedCooldown = 0;
                StopSpeedBoosting();
            }

            speedTxt.text = SPEED_PREFIX + GetSpeed() + ((currentSpeedCooldown == 0)? "" : " (" + currentSpeedCooldown.ToString("0.0") + ")");
        }
    }

    private void UpdateDamageCooldown()
    {
        if (currentDamageCooldown > 0)
        {
            currentDamageCooldown -= Time.deltaTime;

            if (currentDamageCooldown <= 0)
            {
                currentDamageCooldown = 0;
                StopDamageBoosting();
            }

            damageTxt.text = DAMAGE_PREFIX + GetDamage() + ((currentDamageCooldown == 0) ? "" : " (" + currentDamageCooldown.ToString("0.0") + ")");
        }
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

        currentSpeedCooldown = COOLDOWN;
        UpdateSpeedText();
    }

    public void DefineDamageBoost(int damageBoost)
    {
        if (damageMultiplier == 1)
        {
            damageMultiplier = damageBoost;

        }

        currentDamageCooldown = COOLDOWN;
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("3DShooterLose", LoadSceneMode.Single);
    }

    private void UpdateHealthText()
    {
        healthTxt.text = currentHealth + "/" + maxHealth;
    }

    private void UpdateDamageText()
    {
        damageTxt.text = DAMAGE_PREFIX + GetDamage();
    }

    private void UpdateSpeedText()
    {
        speedTxt.text = SPEED_PREFIX + GetSpeed();
    }

    public void UpdateUI()
    {
        UpdateHealthText();
        UpdateDamageText();
        UpdateSpeedText();
    }


}
