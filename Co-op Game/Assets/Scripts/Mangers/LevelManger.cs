using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManger : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    } 
}
