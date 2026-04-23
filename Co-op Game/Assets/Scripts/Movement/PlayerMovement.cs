using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D _rb2D;
    private UIManager _uiManager;
    
    private float _currentX = 0f;
    private float _currentY = 0f;

    public Vector3 Velocity { get; private set; }

    private Vector3 _lastPosition;

    public void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _uiManager = FindObjectOfType<UIManager>();
    }
    
    public void InteractieA()
    {
        Debug.Log("Interact A!");
    }
    
    public void InteractieB()
    {
        Debug.Log("Interact B!");
    }

    public void InteractieY()
    {
        if (_uiManager != null)
        {
            _uiManager.PauseGame();
        }
        else
        {
            Debug.LogWarning("UIManager is niet gevonden in de scene.");
        }
    }
    
    public void MoveWithJoystick(float x, float y)
    {
        _currentX = x;
        _currentY = -y;
    }
    
    private void FixedUpdate()
    {
        var movement = new Vector3(_currentX, _currentY, 0); 
        
        if (movement.sqrMagnitude > 0)
        {
            SetVelocity();
            _rb2D.MovePosition(transform.position + (movement + Velocity) * speed * Time.fixedDeltaTime);
        }
        
        SpriteFlip();
    }

    private void SetVelocity()
    {
        Velocity = (transform.position - _lastPosition) / Time.fixedDeltaTime;
    }
    
    private void SpriteFlip()
    {
        _lastPosition = transform.position;
        var left = new Vector3(-.3f, .3f, 1);
        var right = new Vector3(.3f, .3f, 1);
        var flip = Velocity.x > 0.01f ? left : right;
        
        transform.localScale = new Vector3(flip.x, flip.y, 1);
    }
}
