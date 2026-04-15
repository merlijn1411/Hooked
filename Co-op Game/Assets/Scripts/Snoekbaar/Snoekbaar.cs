using UnityEngine;

public class Snoekbaar : MonoBehaviour
{
    [Header("Particle System")]
    [SerializeField] private ParticleSystem scaredFishesEffect;

    private SpawnSnoekbaar snoekbaarScript;

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
        }
        else
        {
            Instantiate(scaredFishesEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject, 10f);
    }
}
