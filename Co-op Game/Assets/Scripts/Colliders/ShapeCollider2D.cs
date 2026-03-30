using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class ShapeCollider2D : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private bool needsConvert;
    
    private EdgeCollider2D _edgeCollider2D;
    private void Start()
    {
        CreateShape();
        ConvertCollider();
    }

    private void CreateShape()
    {
        _edgeCollider2D = GetComponent<EdgeCollider2D>();
        
        if (_edgeCollider2D == null) gameObject.AddComponent<EdgeCollider2D>();
        
        Debug.Log(_edgeCollider2D);
        var sprite = GetComponent<SpriteRenderer>().sprite;
        
        
        if (sprite == null) return;
        var points = new List<Vector2>();
            
        sprite.GetPhysicsShape(0, points);
            
        points.Add(points[0]); 
            
        _edgeCollider2D.SetPoints(points);

        _edgeCollider2D.edgeRadius = radius;
    }

    private void ConvertCollider()
    {
        var _convertCollider = GetComponent<ConvertCollider2D>();

        if (needsConvert && _convertCollider)
        {
            _convertCollider.Convert(_edgeCollider2D);
        }
    }
}