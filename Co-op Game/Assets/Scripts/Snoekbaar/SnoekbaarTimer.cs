using UnityEngine;
using System.Collections;

public class SnoekbaarTimer : MonoBehaviour
{
    [Header("Timing for spawning")]
    [SerializeField] private float maxTime;
    [SerializeField] private float minTime;

    private SpawnSnoekbaar _spawnSnoekBaarScript;

    private void Start()
    {
        _spawnSnoekBaarScript = GetComponent<SpawnSnoekbaar>();
        StartCoroutine(RandomStartRoutine());
    }

    IEnumerator RandomStartRoutine()
    {
        float delay = Random.Range(minTime, maxTime);
        yield return new WaitForSeconds(delay);

        _spawnSnoekBaarScript.SpawningSnoekbaar();
        StartCoroutine(RandomStartRoutine());
    }

}
