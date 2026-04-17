using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    private Dictionary<string, bool> readyStates = new Dictionary<string, bool>();

    [Header("Player Slots (Images)")]
    public List<Image> playerImages;
    
    public List<string> players = new List<string>();

    [Header("UI Elements")]
    public Button startGameButton;

    private void Start()
    {
        if (startGameButton != null)
        {
            startGameButton.interactable = false;
        }
        
        UpdateUI();
    }

    public void PlayerJoined(string playerId)
    {
        if (players.Count >= 4) return;

        players.Add(playerId);
        
        if (!readyStates.ContainsKey(playerId))
        {
            readyStates[playerId] = false;
        }

        UpdateUI();
        CheckAllReady(); 
    }

    public void PlayerLeft(string playerId)
    {
        players.Remove(playerId);
        if (readyStates.ContainsKey(playerId))
        {
            readyStates.Remove(playerId);
        }

        UpdateUI();
        CheckAllReady(); 
    }

    void UpdateUI()
    {
        for (int i = 0; i < playerImages.Count; i++)
        {
            if (i < players.Count)
            {
                string playerId = players[i];
                bool isReady = readyStates.ContainsKey(playerId) && readyStates[playerId];
                
                playerImages[i].gameObject.SetActive(true);

                Color imgColor = playerImages[i].color;
                
                if (isReady)
                {
                    imgColor.a = 1.0f;
                }
                else
                {
                    imgColor.a = 0.5f;
                }
                
                playerImages[i].color = imgColor;
            }
            else
            {
                playerImages[i].gameObject.SetActive(false);
            }
        }
    }

    public void HandleInput(string playerId, string action)
    {
        if (action == "B")
        {
            ToggleReady(playerId);
        }
    }

    void ToggleReady(string playerId)
    {
        if (!readyStates.ContainsKey(playerId))
        {
            readyStates[playerId] = false;
        }

        readyStates[playerId] = !readyStates[playerId];

        Debug.Log($"✅ Player {playerId} ready: {readyStates[playerId]}");

        UpdateUI();        
        CheckAllReady();
    }

    void CheckAllReady()
    {
        if (startGameButton == null) return;

        if (readyStates.Count == 0 || players.Count == 0)
        {
            startGameButton.interactable = false;
            return;
        }

        foreach (var playerId in players)
        {
            if (!readyStates.ContainsKey(playerId) || !readyStates[playerId])
            {
                startGameButton.interactable = false;
                return;
            }
        }

        Debug.Log("🎉 All players are ready! You can now start the game.");
        startGameButton.interactable = true;
    }

    public void StartGame()
    {
        if (startGameButton != null && !startGameButton.interactable) return;
        
        string sceneToLoad = "MainScene"; // Fallback
        if (LevelManger.Instance != null)
        {
            sceneToLoad = LevelManger.Instance.GetSelectedLevelName();
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
}
