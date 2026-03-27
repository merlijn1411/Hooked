using UnityEngine;

public class GameplayInputHandler : MonoBehaviour , IInputHandler
{
    public PlayerManager playerManager;

    void Start()
    {
        InputRouter.Instance.SetHandler(this);
    }

    public void HandleInput(string playerId, string action)
    {
        playerManager.HandlePlayerInput(playerId, action);
    }
}
