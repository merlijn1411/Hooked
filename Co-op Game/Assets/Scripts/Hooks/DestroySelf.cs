using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    private void OnEnable()
    {
        WinEvent.OnPlayersWon += DestroyItSelf;
    }

    private void OnDisable()
    {
        WinEvent.OnPlayersWon -= DestroyItSelf;
    }

    private void DestroyItSelf()
    {
        Destroy(gameObject);
    }
}
