using System.Collections;
using UnityEngine;

public class SpearRandomizer : MonoBehaviour
{
    [System.Serializable]
    public class SpearData
    {
        public GameObject Prefab;

        public Transform Top;

        public Transform BottomLeft;
        public Transform BottomMiddle;
        public Transform BottomRight;

        public float RotationZ;
    }

    public SpearData LeftToRight;   // ↘ (Middle + Right)
    public SpearData RightToLeft;    // ↙ (Left + Middle)

    public float Speed = 8f;
    public float WaitTime = 1f;
    public float DelayBetweenSpawns = 0.5f;

    private GameObject _currentSpear;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        StartCoroutine(RunLoop());
    }

    private IEnumerator RunLoop()
    {
        bool useLeftToRight = true;

        while (true)
        {
            SpearData spearData = useLeftToRight ? LeftToRight : RightToLeft;

            yield return HandleSpear(spearData, useLeftToRight);

            useLeftToRight = !useLeftToRight;

            yield return new WaitForSeconds(DelayBetweenSpawns);
        }
    }

    private IEnumerator HandleSpear(SpearData spearData, bool leftToRight)
    {
        Vector2 topPos = spearData.Top.position;
        Vector2 bottomPos = GetDirectionalBottom(spearData, leftToRight);

        _currentSpear = Instantiate(spearData.Prefab, topPos, Quaternion.identity);
        _currentSpear.transform.rotation = Quaternion.Euler(0f, 0f, spearData.RotationZ);

        _rigidbody = _currentSpear.GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0;
        _rigidbody.freezeRotation = true;

        yield return MoveTo(bottomPos);

        _rigidbody.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(WaitTime);

        yield return MoveTo(topPos);

        _rigidbody.linearVelocity = Vector2.zero;

        Destroy(_currentSpear);
    }

    private Vector2 GetDirectionalBottom(SpearData data, bool leftToRight)
    {
        Vector2 left = data.BottomLeft.position;
        Vector2 middle = data.BottomMiddle.position;
        Vector2 right = data.BottomRight.position;

        if (leftToRight)
        {
            // ↘ Middle + Right
            return Random.value < 0.5f
                ? Vector2.Lerp(middle, right, Random.value)
                : right;
        }
        else
        {
            // ↙ Left + Middle
            return Random.value < 0.5f
                ? Vector2.Lerp(left, middle, Random.value)
                : left;
        }
    }

    // 🔥 FIXED MOVEMENT (no jitter)
    private IEnumerator MoveTo(Vector2 targetPosition)
    {
        while (true)
        {
            Vector2 currentPosition = _currentSpear.transform.position;
            Vector2 direction = targetPosition - currentPosition;

            float distance = direction.magnitude;

            if (distance < 0.15f)
                break;

            direction.Normalize();

            float moveStep = Speed * Time.fixedDeltaTime;

            if (moveStep > distance)
                moveStep = distance;

            _rigidbody.linearVelocity = direction * (moveStep / Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }

        _rigidbody.linearVelocity = Vector2.zero;
    }
}