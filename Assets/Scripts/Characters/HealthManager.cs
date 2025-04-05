using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Image healthBarFill;
    public TextMeshProUGUI healthText;

    private void Start()
    {
        SetMaxHealth(maxHealth);
    }

    public void SetMaxHealth(int health)
    {
        maxHealth = health;
        currentHealth = health;
        UpdateHealthBar();
    }

    public void UpdateHealth(int newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth); // Prevent out-of-range health
        UpdateHealthBar(); // 🔹 Make sure this is called!

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth; // 🔹 Update UI health bar
        }

        if (healthText != null)
        {
            healthText.text = $"HP: {currentHealth}/{maxHealth}"; // 🔹 Update health text
        }
    }



    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth + "/" + maxHealth;
        }
    }
}