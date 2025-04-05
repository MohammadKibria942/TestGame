using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameSceneManager : MonoBehaviour
{
    public RectTransform[] playerSpawnPoints;
    public RectTransform[] enemySpawnPoints;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public static GameSceneManager Instance { get; private set; }

    public GameObject selectedEnemy = null;
    public bool isPlayerTurn = true;

    private List<GameObject> spawnedPlayers = new List<GameObject>();
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private List<BaseCharacter> turnOrder = new List<BaseCharacter>();
    private int currentTurnIndex = 0;

    private TurnOrderUI turnOrderUI;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        SpawnPlayers();
        SpawnEnemies();

        turnOrder = new List<BaseCharacter>();
        turnOrderUI = FindObjectOfType<TurnOrderUI>();
        turnOrderUI.UpdateTurnOrder(turnOrder, currentTurnIndex);


        // Add all players
        foreach (var player in spawnedPlayers)
        {
            var character = player.GetComponent<BaseCharacter>();
            if (character != null)
                turnOrder.Add(character);
        }

        // Add all enemies
        foreach (var enemy in spawnedEnemies)
        {
            var character = enemy.GetComponent<BaseCharacter>();
            if (character != null)
                turnOrder.Add(character);
        }

        // Sort by speed (highest to lowest)
        turnOrder = turnOrder.OrderByDescending(c => c.speed).ToList();

        // Start first turn
        currentTurnIndex = -1; // So it starts at index 0 after NextTurn()
        NextTurn();

    }

    public void SetTargetEnemy(GameObject enemy)
    {
        if (!isPlayerTurn) return;
        selectedEnemy = enemy;
    }

    private void SpawnPlayers()
    {
        string selectedCharacters = PlayerPrefs.GetString("SelectedCharacters", "");
        if (string.IsNullOrEmpty(selectedCharacters))
        {
            Debug.LogError("No characters found in PlayerPrefs!");
            return;
        }

        string[] characterIDs = selectedCharacters.Split(',');

        for (int i = 0; i < characterIDs.Length && i < playerSpawnPoints.Length; i++)
        {
            RectTransform spawnPoint = playerSpawnPoints[i];

            GameObject playerInstance = Instantiate(playerPrefab);
            playerInstance.name = "Player_" + characterIDs[i];

            AttachToCanvas(playerInstance, spawnPoint.anchoredPosition);

            Cannon cannonScript = playerInstance.GetComponent<Cannon>();
            if (cannonScript != null)
            {
                cannonScript.AssignUIElements();
                cannonScript.UpdateHealthUI();
            }

            spawnedPlayers.Add(playerInstance);
        }
    }

    private void SpawnEnemies()
    {
        int enemyCount = Mathf.Min(enemySpawnPoints.Length, 4);

        for (int i = 0; i < enemyCount; i++)
        {
            RectTransform spawnPoint = enemySpawnPoints[i];

            GameObject enemyInstance = Instantiate(enemyPrefab);
            enemyInstance.name = "Enemy_" + (i + 1);

            AttachToCanvas(enemyInstance, spawnPoint.anchoredPosition);

            Cannon enemyScript = enemyInstance.GetComponent<Cannon>();
            if (enemyScript != null)
            {
                enemyScript.AssignUIElements();
                enemyScript.UpdateHealthUI();
            }

            spawnedEnemies.Add(enemyInstance);
        }
    }

    private void AttachToCanvas(GameObject characterInstance, Vector2 anchoredPosition)
    {
        Transform canvasTransform = GameObject.Find("Canvas")?.transform;
        if (canvasTransform == null) return;

        characterInstance.transform.SetParent(canvasTransform, false);
        characterInstance.transform.localScale = Vector3.one;

        RectTransform rectTransform = characterInstance.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = anchoredPosition;
        }
    }

    private void BuildTurnOrder()
    {
        turnOrder.Clear();

        // Add players
        foreach (GameObject player in spawnedPlayers)
        {
            var character = player.GetComponent<BaseCharacter>();
            if (character != null)
                turnOrder.Add(character);
        }

        // Add enemies
        foreach (GameObject enemy in spawnedEnemies)
        {
            var character = enemy.GetComponent<BaseCharacter>();
            if (character != null)
                turnOrder.Add(character);
        }

        // Sort by speed descending
        turnOrder = turnOrder.OrderByDescending(c => c.speed).ToList();

        currentTurnIndex = 0;
    }

    public void NextTurn()
    {
        if (turnOrder.Count == 0) return;

        currentTurnIndex++;

        // Loop back to start if we reach the end
        if (currentTurnIndex >= turnOrder.Count)
            currentTurnIndex = 0;

        BaseCharacter currentCharacter = turnOrder[currentTurnIndex];

        if (currentCharacter == null || !currentCharacter.gameObject.activeSelf)
        {
            // Skip dead or disabled characters
            NextTurn();
            return;
        }

        // PLAYER TURN
        if (currentCharacter.CompareTag("Player"))
        {
            isPlayerTurn = true;
            Debug.Log("Player's turn: " + currentCharacter.name);
            // Wait for player to act (e.g., spin button)
        }
        else // ENEMY TURN
        {
            isPlayerTurn = false;
            Debug.Log("Enemy's turn: " + currentCharacter.name);
            HandleEnemyTurn(currentCharacter);
        }

        turnOrderUI.UpdateTurnOrder(turnOrder, currentTurnIndex);

    }

    private void HandleEnemyTurn(BaseCharacter enemyCharacter)
    {
        // Pick a random player target
        var alivePlayers = spawnedPlayers
            .Where(p => p.activeSelf)
            .Select(p => p.GetComponent<BaseCharacter>())
            .Where(c => c != null && c.currentHealth > 0)
            .ToList();

        if (alivePlayers.Count == 0)
        {
            Debug.Log("No players left to attack!");
            return;
        }

        BaseCharacter target = alivePlayers[Random.Range(0, alivePlayers.Count)];

        // Enemy attacks
        enemyCharacter.Attack(target);

        // Continue to next turn after short delay
        Invoke(nameof(NextTurn), 1.5f); // Optional delay for animation or feedback
    }
}