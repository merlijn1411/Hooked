using System.Collections;
using UnityEngine;

public class SpearRandomizer : MonoBehaviour
{
    [System.Serializable]
    public class SpearData
    {
        public GameObject Prefab;
        public Transform Top;
        public Transform BottomTarget;
        public float RotationZ;
    }

    [SerializeField] private SpearData leftToRight;
    [SerializeField] private SpearData rightToLeft;

    [SerializeField] private float downSpeed = 14f;
    [SerializeField] private float upSpeed = 4f;

    [SerializeField] private float peekTime = 1f;
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private float delayBetweenSpawns = 0.5f;

    [Header("Start Delay")]
    [SerializeField] private float startDelay = 5f;

    [Header("Audio Clip")]
    [SerializeField] private AudioClip SpearSound;

    private float _speed;
    private GameObject _currentSpear;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        StartCoroutine(RunLoop());
    }

    private IEnumerator RunLoop()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            var spearData = Random.Range(0, 2) == 0
                ? leftToRight
                : rightToLeft;

            yield return HandleSpear(spearData);

            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }

    private IEnumerator HandleSpear(SpearData spearData)
    {
        var topPos = spearData.Top.position;
        var bottomPos = spearData.BottomTarget.position;

        _currentSpear = Instantiate(spearData.Prefab, topPos, Quaternion.identity);
        _currentSpear.transform.rotation =
            Quaternion.Euler(0f, 0f, spearData.RotationZ);

        _rigidbody = _currentSpear.GetComponent<Rigidbody2D>();


        _rigidbody.gravityScale = 0f;
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        _rigidbody.position = topPos;
        _currentSpear.transform.position = topPos;
        Physics2D.SyncTransforms();

        var peekPos = Vector2.Lerp(topPos, bottomPos, 0.2f);

        _speed = downSpeed;
        yield return MoveTo(peekPos);

        StopMovement();
        yield return new WaitForSeconds(peekTime);

        _speed = upSpeed;
        yield return MoveTo(topPos);

        StopMovement();
        yield return new WaitForSeconds(waitTime);

        if (_currentSpear == null)
            yield break;

        SoundManager.Instance.PlaySoundFXClip(SpearSound, transform, 0.6f);

        _speed = downSpeed;
        yield return MoveTo(bottomPos);

        StopMovement();
        yield return new WaitForSeconds(waitTime);

        _speed = upSpeed;
        yield return MoveTo(topPos);

        StopMovement();

        Destroy(_currentSpear);
    }

    private IEnumerator MoveTo(Vector2 targetPosition)
    {
        _rigidbody.linearVelocity = Vector2.zero;

        while (true)
        {
            var currentPosition = _rigidbody.position;

            var direction = targetPosition - currentPosition;
            var distance = direction.magnitude;

            if (distance < 0.15f)
                break;

            direction.Normalize();

            var moveStep = _speed * Time.fixedDeltaTime;

            if (moveStep > distance)
                moveStep = distance;

            _rigidbody.linearVelocity =
                direction * (moveStep / Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }

        _rigidbody.linearVelocity = Vector2.zero;
    }

    private void StopMovement()
    {
        if (_rigidbody != null)
            _rigidbody.linearVelocity = Vector2.zero;
    }
}