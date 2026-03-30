using UnityEngine;

public class ConvertCollider : MonoBehaviour
{
    public void Convert()
    {
        var edge = GetComponent<EdgeCollider2D>();
        if (edge == null) return;

        var poly = gameObject.AddComponent<PolygonCollider2D>();
        
        poly.SetPath(0, edge.points);
        
        // DestroyImmediate(edge); 
    }
}
