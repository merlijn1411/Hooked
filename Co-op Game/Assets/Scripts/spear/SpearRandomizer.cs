using System.Collections;
using UnityEngine;

public class SpearRandomizer : MonoBehaviour
{
    [System.Serializable]
    public class SpearData
    {
        public GameObject Prefab;

        public Transform Top;
        public Transform Bottom;

        public float BottomClampX = 2f; // 🔥 half width van border
        public float BottomClampY = 1f; // 🔥 half height van border

        public float RotationZ;
    }

    public SpearData LeftToRight;   // ↘ 30.39
    public SpearData RightToLeft;    // ↙ -31.2

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
            SpearData data = useLeftToRight ? LeftToRight : RightToLeft;

            yield return HandleSpear(data);

            useLeftToRight = !useLeftToRight;

            yield return new WaitForSeconds(DelayBetweenSpawns);
        }
    }

    private IEnumerator HandleSpear(SpearData data)
    {
        Vector2 topPos = data.Top.position;
        Vector2 bottomPos = GetClampedBottom(data);

        _currentSpear = Instantiate(data.Prefab, topPos, Quaternion.identity);
        _currentSpear.transform.rotation = Quaternion.Euler(0f, 0f, data.RotationZ);

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

    private Vector2 GetClampedBottom(SpearData data)
    {
        Vector2 basePos = data.Bottom.position;

        float offsetX = Random.Range(-data.BottomClampX, data.BottomClampX);
        float offsetY = Random.Range(-data.BottomClampY, data.BottomClampY);

        Vector2 result = new Vector2(basePos.x + offsetX, basePos.y + offsetY);

        // 🔥 clamp zodat hij altijd in border blijft (extra safety)
        result.x = Mathf.Clamp(result.x, basePos.x - data.BottomClampX, basePos.x + data.BottomClampX);
        result.y = Mathf.Clamp(result.y, basePos.y - data.BottomClampY, basePos.y + data.BottomClampY);

        return result;
    }

    private IEnumerator MoveTo(Vector2 targetPosition)
    {
        while (Vector2.Distance(_currentSpear.transform.position, targetPosition) > 0.15f)
        {
            Vector2 direction = (targetPosition - (Vector2)_currentSpear.transform.position).normalized;
            _rigidbody.linearVelocity = direction * Speed;

            yield return null;
        }

        _rigidbody.linearVelocity = Vector2.zero;
    }
}