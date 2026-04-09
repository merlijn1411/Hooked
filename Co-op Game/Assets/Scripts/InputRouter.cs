using UnityEngine;

public class InputRouter : MonoBehaviour
{
    public static InputRouter Instance;

    private IInputHandler currentHandler;

    void Awake()
    {
        Instance = this;
    }

    public void SetHandler(IInputHandler handler)
    {
        currentHandler = handler;
        Debug.Log("🔄 Input switched to: " + handler.GetType().Name);
    }

    public void HandleInput(string playerId, string action, float x = 0f, float y = 0f)
    {
        if (currentHandler != null)
        {
            currentHandler.HandleInput(playerId, action, x, y);
        }
    }
}
