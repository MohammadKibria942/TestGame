using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnOrderUI : MonoBehaviour
{
    public GameObject turnSlotPrefab; // Reference to the TurnSlot TMP Text
    public Transform slotContainer;   // Where to add the slots (TurnOrderPanel)

    private List<GameObject> currentSlots = new List<GameObject>();

    public void UpdateTurnOrder(List<BaseCharacter> turnOrder, int currentTurnIndex)
    {
        // Clear previous UI
        foreach (var slot in currentSlots)
        {
            Destroy(slot);
        }
        currentSlots.Clear();

        // Rebuild slots
        for (int i = 0; i < turnOrder.Count; i++)
        {
            BaseCharacter character = turnOrder[i];

            GameObject slotObj = Instantiate(turnSlotPrefab, slotContainer);
            slotObj.SetActive(true); // Reactivate template clone

            TextMeshProUGUI text = slotObj.GetComponent<TextMeshProUGUI>();
            text.text = character.name;

            // Highlight current turn
            if (i == currentTurnIndex)
            {
                text.color = Color.yellow;
                text.fontStyle = FontStyles.Bold;
            }
            else
            {
                text.color = Color.white;
                text.fontStyle = FontStyles.Normal;
            }

            currentSlots.Add(slotObj);
        }
    }
}
