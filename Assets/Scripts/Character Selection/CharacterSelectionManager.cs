using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    public List<CharacterData> availableCharacters; // List of available characters
    public Transform characterScrollContent; // Scrollable list content panel
    public GameObject characterButtonPrefab; // Prefab for character buttons

    public Transform selectedTeamPanel; // Panel for selected characters
    public GameObject characterSlotPrefab; // Prefab for displaying selected characters

    public Button playButton; // Play button

    private List<CharacterData> selectedCharacters = new List<CharacterData>();

    private void Start()
    {
        playButton.gameObject.SetActive(false); // Hide play button initially
        PopulateCharacterScrollList();
    }

    private void PopulateCharacterScrollList()
    {
        foreach (CharacterData character in availableCharacters)
        {
            GameObject button = Instantiate(characterButtonPrefab, characterScrollContent);
            button.GetComponentInChildren<TextMeshProUGUI>().text = character.characterName;
            button.GetComponent<Image>().sprite = character.characterSprite;
            button.GetComponent<Button>().onClick.AddListener(() => SelectCharacter(character));
        }
    }


    public void SelectCharacter(CharacterData character)
    {
        if (selectedCharacters.Count >= 4)
        {
            Debug.Log("Team is full! Max 4 characters.");
            return;
        }

        // Check if the character is already selected
        if (!selectedCharacters.Contains(character))
        {
            selectedCharacters.Add(character);
            UpdateTeamUI();
        }
    }

    public void RemoveCharacter(CharacterData character)
    {
        if (selectedCharacters.Contains(character))
        {
            selectedCharacters.Remove(character);
            UpdateTeamUI();
        }
    }

    private void UpdateTeamUI()
    {
        // Prevent errors if selectedTeamPanel or characterSlotPrefab is not assigned
        if (selectedTeamPanel == null || characterSlotPrefab == null)
        {
            Debug.LogError("Selected Team Panel or Character Slot Prefab is not assigned in the Inspector!");
            return;
        }

        // Clear the existing team display
        foreach (Transform child in selectedTeamPanel)
        {
            Destroy(child.gameObject);
        }

        // Display selected characters dynamically
        foreach (CharacterData character in selectedCharacters)
        {
            GameObject slot = Instantiate(characterSlotPrefab, selectedTeamPanel);

            // Check if slot was created successfully
            if (slot == null)
            {
                Debug.LogError("Failed to instantiate CharacterSlotPrefab!");
                continue;
            }

            // Assign character name (TextMeshProUGUI)
            TextMeshProUGUI nameText = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (nameText != null)
            {
                nameText.text = character.characterName;
            }
            else
            {
                Debug.LogError("CharacterSlotPrefab is missing a TextMeshProUGUI component!");
            }

            // Assign character image
            Image characterImage = slot.GetComponent<Image>();
            if (characterImage != null)
            {
                characterImage.sprite = character.characterSprite;
            }
            else
            {
                Debug.LogError("CharacterSlotPrefab is missing an Image component!");
            }

            // Add button function to remove character when clicked
            Button removeButton = slot.GetComponent<Button>();
            if (removeButton != null)
            {
                removeButton.onClick.AddListener(() => RemoveCharacter(character));
            }
            else
            {
                Debug.LogError("CharacterSlotPrefab is missing a Button component!");
            }
        }

        // Show play button only if at least 1 character is selected
        if (playButton != null)
        {
            playButton.gameObject.SetActive(selectedCharacters.Count > 0);
        }
        else
        {
            Debug.LogError("Play Button is not assigned in the Inspector!");
        }
    }



    public void StartGame()
    {
        if (selectedCharacters.Count == 0) return; // Prevents starting without characters

        // Save selected character IDs to PlayerPrefs
        PlayerPrefs.SetString("SelectedCharacters", string.Join(",", selectedCharacters.ConvertAll(c => c.characterID.ToString())));
        PlayerPrefs.Save(); // Ensure data is saved

        // Load the Game Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene"); // Ensure this matches your scene name
    }


}
