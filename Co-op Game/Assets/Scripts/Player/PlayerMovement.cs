using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float stepSize = 1f;

    private SpriteRenderer _spriteRenderer;

    private Rigidbody2D _rb2D;
    
    private float _currentX = 0f;
    private float _currentY = 0f;

    public Vector3 Velocity { get; private set; }

    private Vector3 _lastPosition;

    public void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb2D = GetComponent<Rigidbody2D>();
    }

    public void MoveLeft()
    {
        transform.Translate(Vector3.left * stepSize);
    }
    
    public void MoveRight()
    {
        transform.Translate(Vector3.right * stepSize);
    }

    public void Jump()
    {
        Debug.Log("Jump!");
    }
    
    public void MoveWithJoystick(float x, float y)
    {
        _currentX = x;
        _currentY = -y;
    }
    
    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(_currentX, _currentY, 0); 
        
        if (movement.sqrMagnitude > 0)
        {
            SetVelocity();
            // transform.Translate(movement * speed * Time.deltaTime);
            _rb2D.MovePosition(transform.position + (movement + Velocity) * Time.fixedDeltaTime);
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
        var hasFlipped = Velocity.x > 0.01f ? true : false;
        _spriteRenderer.flipX = hasFlipped;
        
    }
}
