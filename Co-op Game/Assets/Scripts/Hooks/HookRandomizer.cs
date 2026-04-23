using System.Collections;
using UnityEngine;

public class HookRandomizer : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 3f;

    [Header("Vertical Settings")]
    [SerializeField] private float minY = -1.29f;
    [SerializeField] private float maxY = 9f;

    [Header("Drop Timing")]
    [SerializeField] private float warningDuration = 4f;

    [Header("Horizontal Settings")]
    [SerializeField] private float horizontalRange = 2f;
    [SerializeField] private float horizontalSpeedMin = 0.5f;
    [SerializeField] private float horizontalSpeedMax = 2f;

    [Header("Line Settings")]
    [SerializeField] private Transform lineStart;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Transform hookTop;

    private GameObject _lineInstance;
    private Transform _lineTransform;
    private SpriteRenderer _lineRenderer;

    private float _lineWidth; 

    private float _verticalSpeed;
    private float _verticalDirection = 1f;

    private Vector3 _startPos;
    private Vector3 _horizontalTarget;
    private float _horizontalSpeed;

    private bool _isWaiting = false;
    private bool _isDropping = false;

    private HookWarningIndecator _warning;

    private void Start()
    {
        _startPos = transform.position;
        _verticalSpeed = Random.Range(minSpeed, maxSpeed);

        ChooseNewHorizontalTarget();

        _warning = GetComponent<HookWarningIndecator>();

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
        Move();
        UpdateLine();
    }

    private void HookRandomFacing()
    {
        var random = Random.Range(1, 10);
        var faceX = random <= 5 ? transform.localScale.x : -transform.localScale.x;
        transform.localScale = new Vector3(faceX, transform.localScale.y);
    }

    private void Move()
    {
        var pos = transform.position;
        pos.y += _verticalDirection * _verticalSpeed * Time.deltaTime;

        if (!_isDropping)
        {
            if (pos.y >= maxY)
            {
                pos.y = maxY;

                if (!_isWaiting)
                {
                    _isWaiting = true;
                    StartCoroutine(DropRoutine());
                }
            }
        }
        else
        {
            if (pos.y <= minY)
            {
                pos.y = minY;

                _isDropping = false;
                _verticalDirection = 1f;
                _isWaiting = false;

                HookDropManager.Instance?.DropFinished();
                ChooseNewHorizontalTarget();
            }
        }

        pos = Vector3.MoveTowards(pos, _horizontalTarget, _horizontalSpeed * Time.deltaTime);
        transform.position = pos;

        _warning?.RefreshIndicatorAt(pos.x);

        if (Vector3.Distance(pos, _horizontalTarget) < 0.05f)
        {
            ChooseNewHorizontalTarget();
        }
    }

    private IEnumerator DropRoutine()
    {
        HookRandomFacing();
        // 🔒 WACHT OP MAX 2
        if (HookDropManager.Instance != null)
            yield return HookDropManager.Instance.RequestDrop();

        if (_warning != null)
            yield return StartCoroutine(_warning.ShowWarning(warningDuration));
        else
            yield return new WaitForSeconds(warningDuration);

        _isDropping = true;
        _verticalDirection = -1f;
    }

    private void ChooseNewHorizontalTarget()
    {
        var newX = _startPos.x + Random.Range(-horizontalRange, horizontalRange);
        _horizontalTarget = new Vector3(newX, transform.position.y, 0);
        _horizontalSpeed = Random.Range(horizontalSpeedMin, horizontalSpeedMax);
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