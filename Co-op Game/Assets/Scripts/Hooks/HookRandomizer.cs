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
    private GameObject _lineInstance;
    private Transform _lineTransform;
    [SerializeField] private float lineFollowSpeed = 5f;

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

        // Line setup
        if (linePrefab != null && lineStart != null)
        {
            _lineInstance = Instantiate(linePrefab, lineStart.position, Quaternion.identity);
            _lineTransform = _lineInstance.transform;
        }
    }

    void Update()
    {
        Move();
        UpdateLine();
    }

    private void HookRandomFacing()
    {
        var random = Random.Range(1, 10);
        Debug.Log(random);
        var faceX = random <= 5 ? transform.localScale.x : -transform.localScale.x;
        Debug.Log(faceX);
        transform.localScale = new Vector3(faceX, transform.localScale.y);
    }

    private void Move()
    {
        Vector3 pos = transform.position;

        // 🔼 OMHOOG of WACHTEN
        if (!_isDropping)
        {
            pos.y += _verticalDirection * _verticalSpeed * Time.deltaTime;

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
            // 🔽 NAAR BENEDEN
            pos.y += _verticalDirection * _verticalSpeed * Time.deltaTime;

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

        // Horizontale movement
        pos = Vector3.MoveTowards(pos, _horizontalTarget, _horizontalSpeed * Time.deltaTime);

        transform.position = pos;

        // Refresh indicator
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

        // ⚠️ WARNING
        if (_warning != null)
            yield return StartCoroutine(_warning.ShowWarning(warningDuration));
        else
            yield return new WaitForSeconds(warningDuration);

        // ⬇️ DROP START
        _isDropping = true;
        _verticalDirection = -1f;
    }

    private void ChooseNewHorizontalTarget()
    {
        float newX = _startPos.x + Random.Range(-horizontalRange, horizontalRange);
        _horizontalTarget = new Vector3(newX, transform.position.y, 0);
        _horizontalSpeed = Random.Range(horizontalSpeedMin, horizontalSpeedMax);
    }

    private void UpdateLine()
    {
        if (lineStart == null || _lineTransform == null) return;

        // Line start volgt X van hook
        var startPos = lineStart.position;
        startPos.x = Mathf.Lerp(startPos.x, transform.position.x, Time.deltaTime * lineFollowSpeed);
        lineStart.position = startPos;

        // Line richting en schaal
        var direction = transform.position - lineStart.position;
        var distance = direction.magnitude;

        _lineTransform.position = lineStart.position;
        _lineTransform.localScale = new Vector3(
            _lineTransform.localScale.x,
            distance,
            _lineTransform.localScale.z
        );
        _lineTransform.up = direction.normalized;
    }
}