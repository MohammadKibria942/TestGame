using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class SlotMachine : MonoBehaviour
{
    public TextMeshProUGUI[] slotTexts;
    public TextMeshProUGUI totalText;
    private int[] currentSlots;
    public List<int>[] slotRows = new List<int>[4]; 
    private bool isSpinning = false; // Prevent multiple calls

    void Start()
    {
        LoadSlotNumbers();
        currentSlots = new int[4];
    }

    public void Spin()
    {
        if (isSpinning) return; // Prevent continuous calls
        isSpinning = true;

        int totalDamage = 0;

        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(0, slotRows[i].Count);
            currentSlots[i] = slotRows[i][randomIndex];
            slotTexts[i].text = currentSlots[i].ToString();
            totalDamage += currentSlots[i];
        }

        totalText.text = "Total: " + totalDamage;
        isSpinning = false; // Allow future spins
    }

    public int CalculateTotal()
    {
        return currentSlots.Sum();
    }

    void LoadSlotNumbers()
    {
        for (int i = 0; i < 4; i++)
        {
            string key = "Row" + i;
            if (PlayerPrefs.HasKey(key))
            {
                string savedData = PlayerPrefs.GetString(key);
                slotRows[i] = savedData.Split(',').Select(int.Parse).ToList();
            }
            else
            {
                slotRows[i] = new List<int> { 1, 2, 3, 4 };
            }
        }
    }

    public int[] GetCurrentSlots()
    {
        return currentSlots;
    }
}
