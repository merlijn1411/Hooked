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
    
    [Header("Level Buttons Container")]
    [Tooltip("Drag the parent object that contains all the level buttons here")]
    [SerializeField] private Transform levelButtonsContainer;

    private Transform[] _levelButtons;

    private int _selectedLevelIndex = 0; 

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

        if (levelButtonsContainer != null)
        {
            _levelButtons = new Transform[levelButtonsContainer.childCount];
            for (int i = 0; i < levelButtonsContainer.childCount; i++)
            {
                _levelButtons[i] = levelButtonsContainer.GetChild(i);
            }
        }
        else
        {
            Debug.LogWarning("⚠️ No Level Buttons Container assigned in the inspector!");
            _levelButtons = new Transform[0];
        }

        SelectLevel(_selectedLevelIndex);
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
    public void SelectLevel(int index)
    {
        if (index < 0 || index >= levels.Length) 
        { 
            return; 
        }
        
        _selectedLevelIndex = index;
        
        if (_levelButtons != null && _levelButtons.Length > 0)
        {
            for (int i = 0; i < _levelButtons.Length; i++)
            {
                if (_levelButtons[i] != null)
                {
                    _levelButtons[i].localScale = Vector3.one;
                }
            }

            if (index < _levelButtons.Length && _levelButtons[index] != null)
            {
                // Scale to the selected size
                _levelButtons[index].localScale = new Vector3(0.8f, 0.8f, 0.8f); 
            }
            else
            {
                Debug.LogWarning($"⚠️ Cannot scale button {index}, it might not exist in the container.");
            }
        }
    }

    public string GetSelectedLevelName()
    {
        if (_selectedLevelIndex >= 0 && _selectedLevelIndex < levels.Length)
        {
            return levels[_selectedLevelIndex];
        }
        return "MainScene"; 
    }
}
