using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManger : MonoBehaviour
{
    public static LevelManger Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject levelSelectionUI;

    [Header("All Levels")]
    [SerializeField] private string[] levels;
    
    [Header("Level Buttons UI (om te schalen)")]
    [SerializeField] private Transform[] levelButtons;

    private int selectedLevelIndex = 0; 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        levelSelectionUI.SetActive(false);
        SelectLevel(selectedLevelIndex);
    }

    public void OpenLevelSelection()
    {
        levelSelectionUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    public void BackToMainMenu()
    {
        mainMenuUI.SetActive(true);
        levelSelectionUI.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Roep deze functie aan op de OnClick van je level knoppen, in plaats van OpenLevel
    public void SelectLevel(int index)
    {
        if (index < 0 || index >= levels.Length) 
        { 
            Debug.LogWarning($"SelectLevel: Index {index} is ongeldig!");
            return; 
        }
        
        selectedLevelIndex = index;
        Debug.Log($"✅ Level geselecteerd: {levels[index]} (Index: {index})");
        
        if (levelButtons != null && levelButtons.Length > 0)
        {
            for (int i = 0; i < levelButtons.Length; i++)
            {
                if (levelButtons[i] != null)
                {
                    levelButtons[i].localScale = Vector3.one;
                }
            }

            if (index < levelButtons.Length && levelButtons[index] != null)
            {
                // Schaal naar de geselecteerde grootte
                levelButtons[index].localScale = new Vector3(0.8f, 0.8f, 0.8f); 
                Debug.Log($"🎨 Knop/Image {index} is groter gemaakt!");
            }
            else
            {
                Debug.LogWarning($"⚠️ Kan knop {index} niet schalen, deze is leeg in de LevelButtons array in de inspector.");
            }
        }
        else
        {
            Debug.LogError("❌ Je hebt nog geen Level Buttons toegevoegd aan de 'Level Buttons' array in de inspector van de LevelManger!");
        }
    }

    // Haalt de naam op van de gekozen map voor de LobbyManager
    public string GetSelectedLevelName()
    {
        if (selectedLevelIndex >= 0 && selectedLevelIndex < levels.Length)
        {
            return levels[selectedLevelIndex];
        }
        return "MainScene"; // Fallback indien er geen is
    }
}
