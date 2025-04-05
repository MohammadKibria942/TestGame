using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NumberSelectionManager : MonoBehaviour
{
    public GameObject numberGridPanel; // The panel that holds the number buttons
    public Button playButton; // The Play button to start the game
    public TextMeshProUGUI selectedNumbersText; // Displays selected numbers
    public Button[] numberButtons; // Buttons for numbers 1-9
    private Dictionary<int, List<int>> rowSelections = new Dictionary<int, List<int>>(); // Stores selections for each row
    private int currentRow = -1; // Tracks which row is being selected
    private int maxSelection = 4; // Max numbers per row

    void Start()
    {
        // Initialize dictionary for each row
        for (int i = 0; i < 4; i++)
        {
            rowSelections[i] = new List<int>();
        }

        // Assign button click event dynamically
        foreach (Button button in numberButtons)
        {
            button.onClick.AddListener(() => SelectNumber(button));
        }

        // Ensure the play button is visible at the start
        playButton.gameObject.SetActive(true);
    }

    // Called when a RowButton is clicked
    public void OpenRowSelection(int rowIndex)
    {
        currentRow = rowIndex; // Set the active row
        numberGridPanel.SetActive(true); // Show the number grid panel
        playButton.gameObject.SetActive(false); // Hide Play button
        UpdateButtonColors(); // Update the button colors
        UpdateSelectedNumbersText(); // Update the displayed selected numbers
    }

    // Handles number selection logic
    public void SelectNumber(Button button)
    {
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText == null) return;

        int number;
        if (int.TryParse(buttonText.text, out number))
        {
            List<int> selectedNumbers = rowSelections[currentRow];

            if (selectedNumbers.Contains(number))
            {
                // Deselect if already selected
                selectedNumbers.Remove(number);
            }
            else if (selectedNumbers.Count < maxSelection)
            {
                // Select new number
                selectedNumbers.Add(number);
            }

            UpdateButtonColors(); // Update button colors
            UpdateSelectedNumbersText(); // Update the selected numbers display
        }
    }

    private void UpdateButtonColors()
    {
        // Reset all button colors to white
        foreach (Button button in numberButtons)
        {
            button.image.color = Color.white;
        }

        // Highlight selected numbers for the active row
        List<int> selectedNumbers = rowSelections[currentRow];
        foreach (Button button in numberButtons)
        {
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                int number;
                if (int.TryParse(buttonText.text, out number) && selectedNumbers.Contains(number))
                {
                    button.image.color = Color.green; // Highlight the selected number
                }
            }
        }
    }

    public void PlayGame()
    {
        // Save each row's selected numbers into PlayerPrefs
        for (int i = 0; i < 4; i++)
        {
            string key = "Row" + i; // Example: "Row0", "Row1", etc.
            if (rowSelections.ContainsKey(i))
            {
                string savedNumbers = string.Join(",", rowSelections[i]); // Convert list to comma-separated string
                PlayerPrefs.SetString(key, savedNumbers);
            }
        }

        PlayerPrefs.Save(); // Save to disk

        // Load the Game Scene
        SceneManager.LoadScene("GameScene"); // Replace with your actual game scene name
    }

    // Updates the displayed selected numbers
    private void UpdateSelectedNumbersText()
    {
        if (currentRow >= 0)
        {
            selectedNumbersText.text = "Selected: " + string.Join(", ", rowSelections[currentRow]);
        }
    }

    // Close button function
    public void CloseNumberGrid()
    {
        numberGridPanel.SetActive(false);
        playButton.gameObject.SetActive(true); // Show Play button again when closing the panel
    }
}
