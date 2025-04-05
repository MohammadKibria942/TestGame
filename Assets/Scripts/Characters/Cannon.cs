using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Cannon : SlotMachineCharacter
{
    public TextMeshProUGUI[] slotTexts;
    public TextMeshProUGUI totalDamageText;
    public Button spinButton;

    //private SlotMachine slotMachine;

    protected override void Start()
    {
        base.Start();

        slotMachine = GetComponent<SlotMachine>();
        if (slotMachine == null)
        {
            Debug.LogError("SlotMachine component is missing on " + gameObject.name);
        }

        AssignUIElements();
        UpdateHealthUI();// Ensure UI shows correct health on spawn
    }

    public void AssignUIElements()
    {
        Transform canvasTransform = transform.Find("Canvas");

        if (canvasTransform == null)
        {
            Debug.LogError("Canvas is missing on " + gameObject.name);
            return;
        }

        Transform uiTransform = canvasTransform.Find("CannonUI");
        if (uiTransform == null)
        {
            Debug.LogError("CannonUI not found inside Canvas on " + gameObject.name);
            return;
        }

        // Health bar
        Transform healthTransform = uiTransform.Find("PlayerHealth/PlayerHealthBar/PlayerHealthBarFill");
        if (healthTransform != null)
        {
            healthBarFill = healthTransform.GetComponent<Image>();
        }

        // Health text
        Transform healthTextTransform = uiTransform.Find("PlayerHealth/PlayerHPText");
        if (healthTextTransform != null)
        {
            healthText = healthTextTransform.GetComponent<TextMeshProUGUI>();
        }

        // Spin Button (only for player)
        if (gameObject.CompareTag("Player"))
        {
            Transform spinButtonTransform = uiTransform.Find("PlayerSpinButton");
            if (spinButtonTransform != null)
            {
                spinButton = spinButtonTransform.GetComponent<Button>();
            }
        }

        // Slot Texts
        slotTexts = new TextMeshProUGUI[4];
        for (int i = 0; i < 4; i++)
        {
            Transform slot = uiTransform.Find($"PlayerSlot{i + 1}");
            if (slot != null)
            {
                slotTexts[i] = slot.GetComponent<TextMeshProUGUI>();
            }
        }

        // Total damage text
        Transform totalTextTransform = uiTransform.Find("PlayerTotalText");
        if (totalTextTransform != null)
        {
            totalDamageText = totalTextTransform.GetComponent<TextMeshProUGUI>();
        }
    }

    public void OnSpinButtonPressed()
    {
        if (GameSceneManager.Instance.selectedEnemy == null)
        {
            Debug.LogWarning("No enemy selected!");
            return;
        }

        // Spin & Display UI
        slotMachine.Spin();
        int[] results = slotMachine.GetCurrentSlots();

        for (int i = 0; i < slotTexts.Length; i++)
        {
            if (slotTexts[i] != null)
                slotTexts[i].text = results[i].ToString();
        }

        int damage = slotMachine.CalculateTotal();
        totalDamageText.text = "Total: " + damage;

        // Deal damage to the selected enemy
        BaseCharacter enemy = GameSceneManager.Instance.selectedEnemy.GetComponent<BaseCharacter>();
        if (enemy != null)
        {
            Attack(enemy); // uses inherited Attack()
        }
        GameSceneManager.Instance.NextTurn();
    }

}
