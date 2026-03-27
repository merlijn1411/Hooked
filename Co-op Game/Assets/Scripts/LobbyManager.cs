using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private Dictionary<string, bool> readyStates = new Dictionary<string, bool>();

    public List<TextMeshProUGUI> playerSlots;

    private List<string> players = new List<string>();
    
    public RectTransform cursor;
    public float cursorSpeed = 500f;

    private Vector2 cursorPosition;

    private void Start()
    {
        UpdateUI();
    }

    public void PlayerJoined(string playerId)
    {
        if (players.Count >= 4) return;

        players.Add(playerId);

        UpdateUI();
    }

    public void PlayerLeft(string playerId)
    {
        players.Remove(playerId);

        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < playerSlots.Count; i++)
        {
            if (i < players.Count)
            {
                playerSlots[i].text = "Player " + (i + 1);
            }
            else
            {
                playerSlots[i].text = "Waiting...";
            }
        }
    }

    public void HandleInput(string playerId, string action)
    {
        if (action == "ready")
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
