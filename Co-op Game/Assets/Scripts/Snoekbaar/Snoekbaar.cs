using UnityEngine;
using System.Collections;

public class Snoekbaar : MonoBehaviour
{
    [Header("Particle System")]
    [SerializeField] private ParticleSystem scaredFishesEffect;

    [Header("Snoekbaar variabelen")]
    [SerializeField] private float ShootSnoekbaarTime;
    [SerializeField] private float snoekbaarSpeed;
    [SerializeField] private float destroySnoekbaar;

    private SpawnSnoekbaar snoekbaarScript;
    private bool isMoving;
    private float moveDirection;

    private void Awake()
    {
        snoekbaarScript = FindAnyObjectByType<SpawnSnoekbaar>();
    }

    private void Start()
    {

        var snoekbaarSpawnLeft = snoekbaarScript.SpawnLeft;

        if (snoekbaarSpawnLeft)
        {
            Instantiate(scaredFishesEffect, transform.position, Quaternion.Euler(new Vector3(0, 0, -180)));
            transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
            moveDirection = 1f;
            StartCoroutine(ShootSnoekbaarRoutine());
        }
        else
        {
            Instantiate(scaredFishesEffect, transform.position, Quaternion.identity);
            transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
            moveDirection = -1f;
            StartCoroutine(ShootSnoekbaarRoutine());
        }
        Destroy(gameObject, destroySnoekbaar);
    }

    private void Update()
    {
        MoveSnoekbaar();
    }

    private IEnumerator ShootSnoekbaarRoutine()
    {
        yield return new WaitForSeconds(ShootSnoekbaarTime);
        isMoving = true;
    }

    private void MoveSnoekbaar()
    {
        if (!isMoving) return;
        transform.position += new Vector3(moveDirection * snoekbaarSpeed * Time.deltaTime, 0, 0);
    }
}
