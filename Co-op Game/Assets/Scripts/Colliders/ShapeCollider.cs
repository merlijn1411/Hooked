using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer), typeof(EdgeCollider2D))]
public class ShapeCollider : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private bool needsConvert;

    private ConvertCollider _convertCollider;
    private EdgeCollider2D _edgeCollider2D;
    private void Start()
    {
        CreateShape();
        ConvertCollider();
    }

    private void CreateShape()
    {
        var sprite = GetComponent<SpriteRenderer>().sprite;
        _edgeCollider2D = GetComponent<EdgeCollider2D>();
        
        if (sprite == null) return;
        var points = new List<Vector2>();
            
        sprite.GetPhysicsShape(0, points);
            
        points.Add(points[0]); 
            
        _edgeCollider2D.SetPoints(points);

        _edgeCollider2D.edgeRadius = radius;
    }

    private void ConvertCollider()
    {
        _convertCollider = GetComponent<ConvertCollider>();

        if (needsConvert && _convertCollider)
        {
            _convertCollider.Convert(_edgeCollider2D);
        }
       
    }
}