using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ShapeEdgeCollider2D : MonoBehaviour
{
    [SerializeField] private SpriteRenderer charRenderer;
    [SerializeField] private float radius = 0.05f;
    [SerializeField] private bool needsConvert;
    
    private EdgeCollider2D _edgeCollider2D;
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        CreateShape();
        CreateShape(_camera);
        ConvertCollider();
    }

    private void CreateShape()
    {
        _edgeCollider2D = GetComponent<EdgeCollider2D>();
        
        var sprite = charRenderer.sprite;
        
        if (sprite == null) return;
        var points = new List<Vector2>();
            
        sprite.GetPhysicsShape(0, points);

        _edgeCollider2D.points = points.ToArray();    
        points.Add(points[0]); 
            
        _edgeCollider2D.SetPoints(points);

        _edgeCollider2D.edgeRadius = radius;
    }

    private void CreateShape(Camera camera)
    {
        if (camera == null) return;
        
        var points = new List<Vector2>();

        if (!camera.orthographic) {Debug.LogError("Camera is not Orthographic, failed to create edge colliders"); return;}

        var bottomLeft = (Vector2)camera.ScreenToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        var topLeft = (Vector2)camera.ScreenToWorldPoint(new Vector3(0, camera.pixelHeight, camera.nearClipPlane));
        var topRight = (Vector2)camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight, camera.nearClipPlane));
        var bottomRight = (Vector2)camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, 0, camera.nearClipPlane));
        
        _edgeCollider2D = gameObject.GetOrAddComponent<EdgeCollider2D>();

        var edgePoints = new [] {bottomLeft,topLeft,topRight,bottomRight, bottomLeft};
        _edgeCollider2D.points = edgePoints;
        points.Add(edgePoints[0]); 
            
        _edgeCollider2D.SetPoints(points);

        _edgeCollider2D.edgeRadius = radius;
    }

    private void ConvertCollider()
    {
        var _convertCollider = gameObject.GetOrAddComponent<ConvertCollider2D>();

        if (needsConvert && _convertCollider)
        {
            _convertCollider.Convert(_edgeCollider2D);
        }
    }
    
}