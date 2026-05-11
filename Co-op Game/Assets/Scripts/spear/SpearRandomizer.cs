using System.Collections;
using UnityEngine;

namespace SpearSystem
{
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

        public SpearData LeftToRight;
        public SpearData RightToLeft;

        public float DownSpeed = 14f;
        public float UpSpeed = 4f;

        public float PeekTime = 1f;
        public float WaitTime = 1f;
        public float DelayBetweenSpawns = 0.5f;

        private float _speed;
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
                SpearData spearData = useLeftToRight
                    ? LeftToRight
                    : RightToLeft;

                yield return HandleSpear(spearData);

                useLeftToRight = !useLeftToRight;

                yield return new WaitForSeconds(DelayBetweenSpawns);
            }
        }

        private IEnumerator HandleSpear(SpearData spearData)
        {
            Vector2 topPos = spearData.Top.position;
            Vector2 bottomPos = spearData.BottomTarget.position;

            _currentSpear = Instantiate(spearData.Prefab, topPos, Quaternion.identity);
            _currentSpear.transform.rotation = Quaternion.Euler(0f, 0f, spearData.RotationZ);

            _rigidbody = _currentSpear.GetComponent<Rigidbody2D>();
            _rigidbody.gravityScale = 0;
            _rigidbody.freezeRotation = true;

            Vector2 peekPos = Vector2.Lerp(topPos, bottomPos, 0.2f);

            _speed = DownSpeed;
            yield return MoveTo(peekPos);

            _rigidbody.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(PeekTime);

            _speed = UpSpeed;
            yield return MoveTo(topPos);

            _rigidbody.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(0.1f);

            _speed = DownSpeed;
            yield return MoveTo(bottomPos);

            _rigidbody.linearVelocity = Vector2.zero;

            yield return new WaitForSeconds(WaitTime);

            _speed = UpSpeed;
            yield return MoveTo(topPos);

            _rigidbody.linearVelocity = Vector2.zero;

            Destroy(_currentSpear);
        }

        private IEnumerator MoveTo(Vector2 targetPosition)
        {
            while (true)
            {
                Vector2 currentPosition = _rigidbody.position;

                Vector2 direction = targetPosition - currentPosition;
                float distance = direction.magnitude;

                if (distance < 0.15f)
                    break;

                direction.Normalize();

                float moveStep = _speed * Time.fixedDeltaTime;

                if (moveStep > distance)
                    moveStep = distance;

                _rigidbody.linearVelocity =
                    direction * (moveStep / Time.fixedDeltaTime);

                yield return new WaitForFixedUpdate();
            }

            _rigidbody.linearVelocity = Vector2.zero;
        }
    }
}