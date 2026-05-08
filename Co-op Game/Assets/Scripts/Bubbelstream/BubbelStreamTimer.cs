using UnityEngine;
using System.Collections;

public class BubbelStreamTimer : MonoBehaviour
{
    [Header("Timing for spawning")]
    [SerializeField] private float maxTime;
    [SerializeField] private float minTime;

    private BubbelStreamSpawner _bubbelStreamSpawnerScript;

    void Start()
    {
        _bubbelStreamSpawnerScript = GetComponent<BubbelStreamSpawner>();
        StartCoroutine(RandomStartRoutine());
    }

    private IEnumerator RandomStartRoutine()
    {
        float delay = Random.Range(minTime, maxTime);
        yield return new WaitForSeconds(delay);

        _bubbelStreamSpawnerScript.SpawnBubbelStream();
        StartCoroutine(RandomStartRoutine());
    }
}
