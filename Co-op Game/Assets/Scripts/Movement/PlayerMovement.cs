using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f; 
    [SerializeField] private float stepSize = 1f;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private float _currentX = 0f;
    private float _currentY = 0f;
    
    public Vector3 Velocity { get; private set; }
    
    private Vector3 _lastPosition;
    
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
    
    private void Update()
    {
        Vector3 movement = new Vector3(_currentX, _currentY, 0); 
        
        if (movement.sqrMagnitude > 0)
        {
            transform.Translate(movement * speed * Time.deltaTime);
        }
        
        SpriteFlip();
    }
    
    private void SpriteFlip()
    {
        Velocity = (transform.position - _lastPosition) / Time.deltaTime;
        _lastPosition = transform.position;
        var hasFlipped = Velocity.x > 0.01f ? true : false;
        _spriteRenderer.flipX = hasFlipped;
        
    }
}
