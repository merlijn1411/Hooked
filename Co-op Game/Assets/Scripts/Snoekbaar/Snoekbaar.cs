using UnityEngine;
using System.Collections;

public class Snoekbaar : MonoBehaviour
{
    [Header("Particle System")]
    [SerializeField] private ParticleSystem scaredFishesEffectLeft;
    [SerializeField] private ParticleSystem scaredFishesEffectRight;

    [Header("Snoekbaar variabelen")]
    [SerializeField] private float ShootSnoekbaarTime;
    [SerializeField] private float snoekbaarSpeed;
    [SerializeField] private float destroySnoekbaar;

    private SpawnSnoekbaar _snoekbaarScript;
    private bool _isMoving;
    private float _moveDirection;

    public void SetSnoek(SpawnSnoekbaar snoekbaar)
    {
        _snoekbaarScript = snoekbaar;
    }

    private void Start()
    {
        Vector3 pos = transform.position;
        pos.z = 0f;
        transform.position = pos;

        var snoekbaarSpawnLeft = _snoekbaarScript.SpawnLeft;

        if (snoekbaarSpawnLeft)
        {
            Instantiate(scaredFishesEffectRight, transform.position, Quaternion.Euler(0,-180,0));
            transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
            _moveDirection = 1f;
        }
        else
        {
            Instantiate(scaredFishesEffectLeft, transform.position, Quaternion.identity);
            transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
            _moveDirection = -1f;
        }
        StartCoroutine(ShootSnoekbaarRoutine());
        Destroy(gameObject, destroySnoekbaar);
    }

    private void Update()
    {
        MoveSnoekbaar();
    }

    private IEnumerator ShootSnoekbaarRoutine()
    {
        yield return new WaitForSeconds(ShootSnoekbaarTime);
        _isMoving = true;
    }

    private void MoveSnoekbaar()
    {
        if (!_isMoving) return;
        transform.position += new Vector3(_moveDirection * snoekbaarSpeed * Time.deltaTime, 0, 0);
    }
}
