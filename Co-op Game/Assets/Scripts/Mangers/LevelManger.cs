using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManger : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject levelSelectionUI;

    [Header("All Levels")]
    [SerializeField] private string[] levels;

    private void Start()
    {
        levelSelectionUI.SetActive(false);
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

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenLevel(int index)
    {
        if (index < 0 || index >= levels.Length) { return; }
        SceneManager.LoadScene(levels[index]);
    }
}
