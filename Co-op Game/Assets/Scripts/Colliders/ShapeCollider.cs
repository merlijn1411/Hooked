using System;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer), typeof(EdgeCollider2D))]
public class ShapeCollider : MonoBehaviour
{
    [SerializeField] private float radius;
    
    private void Start()
    {
        var sprite = GetComponent<SpriteRenderer>().sprite;
        var edgeCollider = GetComponent<EdgeCollider2D>();

        if (sprite == null) return;
        var points = new List<Vector2>();
            
        sprite.GetPhysicsShape(0, points);
            
        points.Add(points[0]); 
            
        edgeCollider.SetPoints(points);

        edgeCollider.edgeRadius = radius;
    }
}