using UnityEngine;

public class HookRandomizer : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 3f;

    [Header("Vertical Settings")]
    [SerializeField] private float minY = -1.29f;
    [SerializeField] private float maxY = 9f;
    [SerializeField] private float verticalCooldown = 1f;

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

    private Vector3 _startPos;
    private float _verticalDirection;
    private float _verticalSpeed;

    private Vector3 _horizontalTarget;
    private float _horizontalSpeed;

    private float _verticalCooldownTimer = 0f;

    private HookRandomizer[] _allHooks;

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
                _verticalDirection = -1f;
                _verticalCooldownTimer = verticalCooldown;
                ChooseNewHorizontalTarget();
            }
            else if (pos.y <= minY)
            {
                pos.y = minY;
                _verticalDirection = 1f;
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


        if (Vector3.Distance(pos, _horizontalTarget) < 0.01f)
        {
            ChooseNewHorizontalTarget();
        }

    
        if (_lineTransform != null && lineStart != null)
        {
            Vector3 direction = pos - lineStart.position;
            float distance = direction.magnitude;

            _lineTransform.position = lineStart.position; // begint exact bovenaan
            _lineTransform.localScale = new Vector3(
                _lineTransform.localScale.x,
                distance, // GEEN 0.5 meer!
                _lineTransform.localScale.z);

            _lineTransform.up = direction.normalized;
        }
    }

    private void ChooseNewHorizontalTarget()
    {
        float newX = _startPos.x + Random.Range(-horizontalRange, horizontalRange);
        _horizontalTarget = new Vector3(newX, transform.position.y, 0);
        _horizontalSpeed = Random.Range(horizontalSpeedMin, horizontalSpeedMax);
    }
}