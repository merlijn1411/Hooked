using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinListener : MonoBehaviour
{

    private void OnEnable()
    {
        WinEvent.OnPlayersWon += OnWin;
    }

    private void OnDisable()
    {
        WinEvent.OnPlayersWon -= OnWin;
    }

    //In this function you can choose what happens when the players win.
    private void OnWin()
    {
        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Main Menu");
    }
}