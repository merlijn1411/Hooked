using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManger : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject levelSelectionUI;

    private void Start()
    {
        levelSelectionUI.SetActive(false);
    }

    public void OpenLevelSelection()
    {
        levelSelectionUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
