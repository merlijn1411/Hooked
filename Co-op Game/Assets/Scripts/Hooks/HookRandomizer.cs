using System.Collections;
using UnityEngine;

public class HookRandomizer : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    [Header("Vertical Settings")]
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [Header("Drop Timing")]
    [SerializeField] private float warningDuration;

    [Header("Horizontal Settings")]
    [SerializeField] private float horizontalRange;
    [SerializeField] private float horizontalSpeedMin;
    [SerializeField] private float horizontalSpeedMax;

    [Header("Random Drop Delay")]
    [SerializeField] private float minRandomDropDelay;
    [SerializeField] private float maxRandomDropDelay;

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
    }

    private void Update()
    {
        Move();
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

                _verticalSpeed = Random.Range(minSpeed, maxSpeed);

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

        float randomDelay = Random.Range(minRandomDropDelay, maxRandomDropDelay);
        yield return new WaitForSeconds(randomDelay);

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
}