using UnityEngine;

public class WinListener : MonoBehaviour
{
    [SerializeField] private ParticleSystem confettiEffect;

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
        Debug.Log("You Won!");
        Instantiate(confettiEffect);
    }
}
