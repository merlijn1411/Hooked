using UnityEngine;

public class GameplayInputHandler : MonoBehaviour, IInputHandler
{
    public PlayerManager playerManager;

    private void Start()
    {
        if (InputRouter.Instance != null)
        {
            InputRouter.Instance.SetHandler(this);
        }
    }
    
    private void OnEnable()
    {
        if (InputRouter.Instance != null)
        {
            InputRouter.Instance.SetHandler(this);
        }
    }

    public void HandleInput(string playerId, string action, float x = 0f, float y = 0f)
    {
        playerManager.HandlePlayerInput(playerId, action, x, y);
    }
}
