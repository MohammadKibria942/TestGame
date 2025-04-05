using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Important for UI click detection

public class EnemyTargeting : MonoBehaviour, IPointerClickHandler
{
    public static EnemyTargeting currentTarget; // Holds the selected target
    private Image enemyImage; // UI Image component for highlighting
    private Color originalColor; // Stores the default color

    private void Start()
    {
        // Try getting the Image component (for UI-based highlighting)
        enemyImage = GetComponent<Image>();

        if (enemyImage != null)
        {
            originalColor = enemyImage.color; // Store original color
        }
        else
        {
            Debug.LogError("❌ Image component missing on: " + gameObject.name);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameSceneManager.Instance.isPlayerTurn) return; // Ensure clicking only works on the player's turn

        // 🔹 Find the closest parent with BaseCharacter
        BaseCharacter enemyCharacter = GetComponentInParent<BaseCharacter>();

        if (enemyCharacter == null)
        {
            Debug.LogError("❌ Selected enemy does NOT have BaseCharacter: " + gameObject.name);
            return;
        }

        // ✅ Select the correct enemy object
        GameSceneManager.Instance.SetTargetEnemy(enemyCharacter.gameObject);

        Debug.Log("✔ Targeted Enemy: " + enemyCharacter.gameObject.name);
        SelectEnemy();
    }

    public void SelectEnemy()
    {
        if (currentTarget != null)
        {
            currentTarget.DeselectEnemy(); // Remove highlight from previous selection
        }

        currentTarget = this;

        if (enemyImage != null)
        {
            enemyImage.color = Color.red; // Highlight selected enemy
        }
    }

    public void DeselectEnemy()
    {
        if (enemyImage != null)
        {
            enemyImage.color = originalColor; // Reset to original color
        }
    }
}
