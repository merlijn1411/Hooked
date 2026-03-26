using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector3 Velocity { get; private set; }
    
    [SerializeField] private float speed;
    
    private SpriteRenderer _spriteRenderer;
    private Vector3 _lastPosition;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        var inputX = Input.GetAxis("Horizontal");
        var inputY = Input.GetAxis("Vertical"); 
        
        var tempTrans = new Vector3(inputX, inputY, 0);
        tempTrans = tempTrans.normalized * speed * Time.deltaTime;

        transform.position += tempTrans;
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
