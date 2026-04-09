using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
            Debug.Log("👑 Host player set to: " + _hostPlayerId);
        }

        if (playerId == _hostPlayerId)
        {
            if (action == "joystick")
            {
                _currentJoystickX = x;
                _currentJoystickY = -y;  
            }

            if (action == "A")
            {
                SimulateCursorClick();
            }
        }

        if (action == "B")
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

    private void SimulateCursorClick()
    {
        if (cursor == null || EventSystem.current == null) return;

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = cursor.position
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                Button clickedButton = result.gameObject.GetComponentInParent<Button>();

                if (clickedButton != null && clickedButton.interactable)
                {
                    Debug.Log("🖱️ Cursor clicked button: " + clickedButton.gameObject.name);
                    clickedButton.onClick.Invoke();
                    break;
                }
            }
        }
    }
}
