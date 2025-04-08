using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleEndManager : MonoBehaviour
{
    public GameObject resultPanel;// Assign the BattleEndPanel in Inspector
    public TextMeshProUGUI resultText;// Assign the result text ("You Win!" / "You Lose!")
    public Button backToMenuButton;// Assign the Back to Menu button

    public static BattleEndManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        resultPanel.SetActive(false); // Hide result panel at start

        // Optionally add listener for back button if not done in the Inspector
        if (backToMenuButton != null)
            backToMenuButton.onClick.AddListener(OnBackToMenu);
    }

    public void ShowResult(bool isWin)
    {
        resultPanel.SetActive(true);
        resultText.text = isWin ? "You Win!" : "You Lose!";
        Time.timeScale = 0f; // Pause the game
    }

    public void OnBackToMenu()
    {
        Time.timeScale = 1f; // Resume time before loading
        SceneManager.LoadScene("CharacterSelectionScene"); // Change this if your scene name is different
    }
}
