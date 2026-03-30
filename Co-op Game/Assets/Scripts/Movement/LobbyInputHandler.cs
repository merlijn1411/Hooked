using UnityEngine;

public class LobbyInputHandler : MonoBehaviour, IInputHandler 
{
    public LobbyManager lobbyManager;

    [Header("Cursor Settings")]
    public RectTransform cursor;          
    public float cursorSpeed = 500f;       
    
    private string _hostPlayerId;
    
    private float _currentJoystickX = 0f;
    private float _currentJoystickY = 0f;

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
        if (string.IsNullOrEmpty(_hostPlayerId) && lobbyManager != null && lobbyManager.players.Count > 0)
        {
            _hostPlayerId = lobbyManager.players[0];
            Debug.Log("🎮 Host player set to: " + _hostPlayerId);
        }

        if (playerId == _hostPlayerId)
        {
            // Als de actie van de joystick komt, slaan we x en y op. 
            //(Zorg ervoor dat the string in je PhoneInputManager overeenkomt! bijv: "joystick")
            if (action == "joystick")
            {
                _currentJoystickX = x;
                _currentJoystickY = -y;  
            }
        }

        if (action == "ready")
        {
            lobbyManager.HandleInput(playerId, action);
        }

        if (action == "leave")
        {
            lobbyManager.PlayerLeft(playerId);
        }
    }

    private void Update()
    {
        if (cursor != null)
        {
            Vector3 movement = new Vector3(_currentJoystickX, _currentJoystickY, 0) * cursorSpeed * Time.deltaTime;
            
            cursor.anchoredPosition += (Vector2)movement;
        }
    }
}
