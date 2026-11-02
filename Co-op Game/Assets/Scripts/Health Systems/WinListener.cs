using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinListener : MonoBehaviour
{
    [Header("Audio Clip")]
    [SerializeField] private AudioClip wonSound;

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
        SoundManager.Instance.PlaySoundFXClip(wonSound, transform, 1f);
        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Main Menu");
    }
}