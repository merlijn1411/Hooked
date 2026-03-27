using UnityEngine;

public class LobbyInputHandler : MonoBehaviour, IInputHandler 
{
    public LobbyManager lobbyManager;

    private void Start()
    {
        if (InputRouter.Instance != null)
        {
            InputRouter.Instance.SetHandler(this);
        }
    }

    public void HandleInput(string playerId, string action)
    {
        if (action == "ready")
        {
            lobbyManager.HandleInput(playerId, action);
        }

        if (action == "leave")
        {
            lobbyManager.PlayerLeft(playerId);
        }
    }
}
