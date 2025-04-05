using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public BaseCharacter player;
    public BaseCharacter enemy;

    public TextMeshProUGUI turnCounterText;
    public TextMeshProUGUI gameStatusText;
    public GameObject gameOverPanel; // Panel for win/loss message
    public TextMeshProUGUI gameOverText; // Text inside the panel

    private int turnCounter = 1;
    private bool isPlayerTurn = true;

    private void Start()
    {
        turnCounterText.text = "Turn: 1";
        gameStatusText.text = "Player's Turn";
        gameOverPanel.SetActive(false); // Hide game over panel at start
    }

    public void OnPlayerAttackButtonClicked()
    {
        if (isPlayerTurn)
        {
            player.Attack(enemy); // Uses Attack() method in BaseCharacter
            CheckGameOver();

            if (enemy.currentHealth > 0) // Only proceed if enemy is still alive
            {
                gameStatusText.text = "Enemy's Turn";
                isPlayerTurn = false;
                turnCounter++;
                turnCounterText.text = "Turn: " + turnCounter;
                Invoke("EnemyTurn", 1.5f);
            }
        }
    }

    private void EnemyTurn()
    {
        enemy.Attack(player); // Enemy attacks player
        CheckGameOver();

        if (player.currentHealth > 0) // Only proceed if player is still alive
        {
            gameStatusText.text = "Player's Turn";
            isPlayerTurn = true;
        }
    }

    private void CheckGameOver()
    {
        if (player.currentHealth <= 0)
        {
            ShowGameOverScreen("You Lost!");
        }
        else if (enemy.currentHealth <= 0)
        {
            ShowGameOverScreen("You Won!");
        }
    }

    private void ShowGameOverScreen(string message)
    {
        gameOverText.text = message;
        gameOverPanel.SetActive(true);
        Invoke("ReturnToNumberSelectionScene", 3f); // Return after 3 seconds
    }

    private void ReturnToNumberSelectionScene()
    {
        SceneManager.LoadScene("NumbersSelectionScene"); // Adjust scene name if necessary
    }
}
