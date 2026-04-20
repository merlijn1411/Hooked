using UnityEngine;

public class ConvertCollider2D : MonoBehaviour
{
    public void Convert(EdgeCollider2D edge)
    {
        if (edge == null) return;
        
        var outer = edge.points;
        
        if (outer[0] != outer[^1])
        {
            var closed = new Vector2[outer.Length + 1];
            outer.CopyTo(closed, 0);
            closed[^1] = outer[0];
            outer = closed;
        }
        
        var inner = ScaleInwards(outer, .95f);
        
        var poly = gameObject.AddComponent<PolygonCollider2D>();

        poly.pathCount = 2;
        
        poly.SetPath(0, outer);
        
        System.Array.Reverse(inner);
        poly.SetPath(1, inner);

        poly.isTrigger = edge.isTrigger;
        
        Destroy(edge);
    }
    
    private Vector2[] ScaleInwards(Vector2[] points, float scale)
    {
        var center = Vector2.zero;

        foreach (var p in points)
            center += p;

        center /= points.Length;

        var result = new Vector2[points.Length];

        for (var i = 0; i < points.Length; i++)
        {
            result[i] = center + (points[i] - center) * scale;
        }

        return result;
    }
}
