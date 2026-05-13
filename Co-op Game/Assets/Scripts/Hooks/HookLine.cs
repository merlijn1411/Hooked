using UnityEngine;

public class HookLine : MonoBehaviour
{
    [Header("Line Settings")]
    [SerializeField] private Transform lineStart;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Transform hookTop;

    private GameObject _lineInstance;
    private Transform _lineTransform;
    private SpriteRenderer _lineRenderer;

    private float _lineWidth;

    private void Start()
    {
        if (linePrefab != null && lineStart != null)
        {
            _lineInstance = Instantiate(linePrefab, lineStart.position, Quaternion.identity);
            _lineTransform = _lineInstance.transform;

            _lineRenderer = _lineTransform.GetComponent<SpriteRenderer>();

            if (_lineRenderer != null)
            {
                _lineRenderer.drawMode = SpriteDrawMode.Tiled;

                _lineWidth = _lineRenderer.size.x;

                _lineWidth *= _lineTransform.localScale.x;
            }
        }
    }

    private void Update()
    {
        UpdateLine();
    }

    public void SetLineStart(Transform startPoint)
    {
        lineStart = startPoint;
    }

    private void UpdateLine()
    {
        if (lineStart == null || _lineTransform == null || hookTop == null) return;

        Vector3 startPos = new Vector3(transform.position.x, lineStart.position.y, 0);
        lineStart.position = startPos;

        Vector3 endPos = hookTop.position;
        Vector3 direction = endPos - startPos;
        float distance = direction.magnitude;

        if (distance <= 0.001f) return;

        _lineTransform.position = startPos;
        _lineTransform.up = direction.normalized;

        if (_lineRenderer != null)
        {
            _lineRenderer.drawMode = SpriteDrawMode.Tiled;

            _lineRenderer.size = new Vector2(_lineWidth, distance);
        }
    }
}
