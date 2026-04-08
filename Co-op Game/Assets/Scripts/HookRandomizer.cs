using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookRandomizer : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 3f;

    [Header("Vertical Settings")]
    [SerializeField] private float minY = -1.29f;
    [SerializeField] private float maxY = 9f;

    [Header("Drop Cooldown")]
    [SerializeField] private float verticalCooldown = 7f;

    private const float DefaultCooldownMin = 5f;
    private const float DefaultCooldownMax = 9f;

    [Header("Horizontal Settings")]
    [SerializeField] private float horizontalRange = 2f;
    [SerializeField] private float horizontalSpeedMin = 0.5f;
    [SerializeField] private float horizontalSpeedMax = 2f;

    [Header("Spacing")]
    [SerializeField] private float minDistance = 1f;

    [Header("Line Settings")]
    [SerializeField] private Transform lineStart;
    [SerializeField] private GameObject linePrefab;
    private GameObject _lineInstance;
    private Transform _lineTransform;

    [Header("Line Follow Settings")]
    [SerializeField] private float lineFollowSpeed = 5f;

    private Vector3 _startPos;
    private float _verticalDirection;
    private float _verticalSpeed;

    private Vector3 _horizontalTarget;
    private float _horizontalSpeed;

    private float _verticalCooldownTimer = 0f;

    private HookRandomizer[] _allHooks;
    private HookWarningIndecator _warning;

    private bool _isWaitingForDrop = false;

    private static int s_activeDrops = 0;
    private const int MaxConcurrentDrops = 2;
    // FIFO queue to ensure only MaxConcurrentDrops start dropping and to avoid race conditions
    private static readonly Queue<HookRandomizer> s_dropQueue = new Queue<HookRandomizer>();
    private bool _isDropping = false;

    void Start()
    {
        _startPos = transform.position;

        _verticalDirection = Random.value < 0.5f ? -1f : 1f;
        _verticalSpeed = Random.Range(minSpeed, maxSpeed);

        ChooseNewHorizontalTarget();

        _allHooks = FindObjectsByType<HookRandomizer>(FindObjectsSortMode.None);

        foreach (var hook in _allHooks)
        {
            if (hook == this) continue;
            if (Vector3.Distance(transform.position, hook.transform.position) < minDistance)
            {
                transform.position += new Vector3(minDistance, 0, 0);
            }
        }

        if (linePrefab != null && lineStart != null)
        {
            _lineInstance = Instantiate(linePrefab, lineStart.position, Quaternion.identity);
            _lineTransform = _lineInstance.transform;
        }

        _warning = GetComponent<HookWarningIndecator>();

        // initialize per-hook randomized cooldown in the 5-9s range
        verticalCooldown = Random.Range(DefaultCooldownMin, DefaultCooldownMax);
        _verticalCooldownTimer = verticalCooldown;
    }

    void Update()
    {
        HandleMovementAndLine();
    }

    private void HandleMovementAndLine()
    {
        Vector3 pos = transform.position;

        if (_verticalCooldownTimer > 0f)
        {
            _verticalCooldownTimer -= Time.deltaTime;
        }
        else
        {
            pos.y += _verticalDirection * _verticalSpeed * Time.deltaTime;

            if (pos.y >= maxY)
            {
                pos.y = maxY;

                // ensure transform updated so indicator spawns at correct x
                transform.position = pos;

                if (!_isWaitingForDrop)
                {
                    _isWaitingForDrop = true;
                    StartCoroutine(DropRoutine());
                }

                return;
            }
            else if (pos.y <= minY)
            {
                pos.y = minY;

                if (_isDropping)
                {
                    _isDropping = false;
                    s_activeDrops = Mathf.Max(0, s_activeDrops - 1);
                }

                _verticalDirection = 1f;

                // assign a single cooldown value (randomized between 5 and 9s)
                verticalCooldown = Random.Range(DefaultCooldownMin, DefaultCooldownMax);
                _verticalCooldownTimer = verticalCooldown;

                ChooseNewHorizontalTarget();
            }
        }

        pos = Vector3.MoveTowards(pos, _horizontalTarget, _horizontalSpeed * Time.deltaTime);

        foreach (var hook in _allHooks)
        {
            if (hook == this) continue;
            if (Vector3.Distance(pos, hook.transform.position) < minDistance)
            {
                pos = transform.position;
                break;
            }
        }

        transform.position = pos;

        _warning?.RefreshIndicatorAt(pos.x);

        if (Vector3.Distance(pos, _horizontalTarget) < 0.01f)
        {
            ChooseNewHorizontalTarget();
        }

        if (lineStart != null)
        {
            Vector3 startPos = lineStart.position;
            startPos.x = Mathf.Lerp(startPos.x, pos.x, Time.deltaTime * lineFollowSpeed);
            lineStart.position = startPos;
        }

        if (_lineTransform != null && lineStart != null)
        {
            Vector3 direction = pos - lineStart.position;
            float distance = direction.magnitude;

            _lineTransform.position = lineStart.position;
            _lineTransform.localScale = new Vector3(
                _lineTransform.localScale.x,
                distance,
                _lineTransform.localScale.z);

            _lineTransform.up = direction.normalized;
        }
    }

    private IEnumerator DropRoutine()
    {
        if (_warning != null)
        {
            // ShowWarning instantiates and waits warningTime, but does NOT destroy the indicator.
            // After ShowWarning returns, indicator should still exist until we explicitly hide it below.
            yield return StartCoroutine(_warning.ShowWarning(null));
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }

        // Enqueue and wait until this hook is at the front AND there is a free slot.
        s_dropQueue.Enqueue(this);
        while (s_dropQueue.Peek() != this || s_activeDrops >= MaxConcurrentDrops)
            yield return null;
        // this coroutine is now allowed to start dropping
        s_dropQueue.Dequeue();

        s_activeDrops++;
        _isDropping = true;

        // Hide the indicator at the exact moment the hook goes down.
        _warning?.HideIndicator();          

        _verticalDirection = -1f;
        _isWaitingForDrop = false;
    }

    private void ChooseNewHorizontalTarget()
    {
        float newX = _startPos.x + Random.Range(-horizontalRange, horizontalRange);
        _horizontalTarget = new Vector3(newX, transform.position.y, 0);
        _horizontalSpeed = Random.Range(horizontalSpeedMin, horizontalSpeedMax);
    }
}