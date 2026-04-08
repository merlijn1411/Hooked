using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private Dictionary<string, bool> readyStates = new Dictionary<string, bool>();

    public List<TextMeshProUGUI> playerSlots;

    public List<string> players = new List<string>();

    private void Start()
    {
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
    }

    public void PlayerLeft(string playerId)
    {
        players.Remove(playerId);
        if (readyStates.ContainsKey(playerId))
        {
            readyStates.Remove(playerId);
        }

        UpdateUI();
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
                playerSlots[i].color = Color.gray; 
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
        if (readyStates.Count == 0 || readyStates.Count != players.Count) return;

        foreach (var player in readyStates)
        {
            if (!player.Value) return;
        }

        Debug.Log("🚀 All players ready → start game!");
        StartGame();
    }

    void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
