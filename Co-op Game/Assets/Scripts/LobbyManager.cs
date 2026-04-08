using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class LobbyManager : MonoBehaviour
{
    private Dictionary<string, bool> readyStates = new Dictionary<string, bool>();

    public List<TextMeshProUGUI> playerSlots;
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
        for (int i = 0; i < playerSlots.Count; i++)
        {
            if (i < players.Count)
            {
                string playerId = players[i];
                bool isReady = readyStates.ContainsKey(playerId) && readyStates[playerId];
                
                if (isReady)
                {
                    playerSlots[i].text = "Player " + (i + 1) + " (Ready!)";
                    playerSlots[i].color = Color.green;
                }
                else
                {
                    playerSlots[i].text = "Player " + (i + 1);
                    playerSlots[i].color = Color.white;
                }
            }
            else
            {
                playerSlots[i].text = "Waiting...";
                playerSlots[i].color = Color.white; 
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

    // Koppel deze methode aan the "On Click ()" event in de Inspector van je UI Unity Button!
    public void StartGame()
    {
        // Ingebouwde check mocht the button op de een of andere manier gehackt of per ongeluk ge-clickt worden
        if (startGameButton != null && !startGameButton.interactable) return;

        Debug.Log("🚀 the Host has started the game!");
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}
