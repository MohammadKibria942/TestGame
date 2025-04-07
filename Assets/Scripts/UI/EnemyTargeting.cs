using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnemyTargeting : MonoBehaviour, IPointerClickHandler
{
    public static EnemyTargeting currentTarget;
    private Image enemyImage;
    private Color originalColor;

    private void Start()
    {
        enemyImage = GetComponent<Image>();

        if (enemyImage != null)
        {
            originalColor = enemyImage.color;
        }
        else
        {
            Debug.LogError("Image component missing on: " + gameObject.name);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameSceneManager.Instance.isPlayerTurn) return;

        BaseCharacter enemyCharacter = GetComponentInParent<BaseCharacter>();

        if (enemyCharacter == null)
        {
            Debug.LogError("Selected enemy does NOT have BaseCharacter: " + gameObject.name);
            return;
        }

        GameSceneManager.Instance.SetTargetEnemy(enemyCharacter.gameObject);
        SelectEnemy();
    }

    public void SelectEnemy()
    {
        if (currentTarget != null)
        {
            currentTarget.DeselectEnemy();
        }

        currentTarget = this;

        if (enemyImage != null)
        {
            enemyImage.color = Color.red;
        }
    }

    public void DeselectEnemy()
    {
        if (enemyImage != null)
        {
            enemyImage.color = originalColor;
        }
    }
}
