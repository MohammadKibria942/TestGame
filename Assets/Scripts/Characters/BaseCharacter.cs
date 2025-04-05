using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BaseCharacter : MonoBehaviour
{
    public int maxHealth = 100;
    public int speed = 5; // Default speed, you can override in prefabs
    public int currentHealth;

    [Header("UI Elements")]
    public Image healthBarFill;// UI health bar
    public TextMeshProUGUI healthText;// UI HP text

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public virtual void Attack(BaseCharacter target)
    {
        Debug.Log("Base attack - should be overridden by child classes.");
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"HP: {currentHealth}/{maxHealth}";
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " has been defeated");
        gameObject.SetActive(false);// Placeholder death behavior
    }
}
