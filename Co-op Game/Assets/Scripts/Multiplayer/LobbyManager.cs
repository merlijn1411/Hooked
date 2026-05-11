using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private CharacterDatabase characterDatabase;
    
    private Dictionary<string, bool> _readyStates = new Dictionary<string, bool>();

    [Header("Player Slots (Images)")]
    public List<Image> PlayerImages;
    
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

    public void PlayerJoined(string playerId, int playerIndex)
    {
        FileManager.Instance.WriteFile(playerIndex, new ExternalVariables
            {
                ID = playerId,
                Index = playerIndex
        }, 0);
        
        if (players.Contains(playerId))
        {
            Debug.Log($"Speler {playerId} is al in de lobby. (Reconnect)");
            
            if (!_readyStates.ContainsKey(playerId))
            {
                _readyStates[playerId] = false;
            }
            
            UpdateUI();
            CheckAllReady();
            return; 
        }

        if (players.Count >= 4) return;

        players.Add(playerId);
        
        if (!_readyStates.ContainsKey(playerId))
        {
            _readyStates[playerId] = false;
        }

        UpdateUI();
        CheckAllReady(); 
    }

    public void PlayerLeft(string playerId)
    {
        players.Remove(playerId);
        if (_readyStates.ContainsKey(playerId))
        {
            _readyStates.Remove(playerId);
        }

        UpdateUI();
        CheckAllReady(); 
    }

    void UpdateUI()
    {
        for (int i = 0; i < PlayerImages.Count; i++)
        {
            if (i < players.Count)
            {
                string playerId = players[i];
                bool isReady = _readyStates.ContainsKey(playerId) && _readyStates[playerId];
                
                PlayerImages[i].gameObject.SetActive(true);

                Color imgColor = PlayerImages[i].color;

                var readtAlpha = isReady ? 1.0f : 0.5f; 
                
                imgColor.a = readtAlpha;
                
                PlayerImages[i].color = imgColor;
            }
            else
            {
                PlayerImages[i].gameObject.SetActive(false);
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
        if (!_readyStates.ContainsKey(playerId))
        {
            _readyStates[playerId] = false;
        }

        _readyStates[playerId] = !_readyStates[playerId];

        Debug.Log($"✅ Player {playerId} ready: {_readyStates[playerId]}");

        UpdateUI();        
        CheckAllReady();
    }

    void CheckAllReady()
    {
        if (startGameButton == null) return;

        if (_readyStates.Count == 0 || players.Count == 0)
        {
            startGameButton.interactable = false;
            return;
        }

        foreach (var playerId in players)
        {
            if (!_readyStates.ContainsKey(playerId) || !_readyStates[playerId])
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
        
        string sceneToLoad = "MainScene"; 
        if (LevelManger.Instance != null)
        {
            sceneToLoad = LevelManger.Instance.GetSelectedLevelName();
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
    
}
