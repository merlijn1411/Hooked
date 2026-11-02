using UnityEngine;
using System.Collections;

public class BaitSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject baitPrefab;

    [Header("Health Reference")]
    [SerializeField] private PlayersHealth playersHealth;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDelay = 5f;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float spawnY;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnBait();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnBait()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);

        GameObject baitObject = Instantiate(baitPrefab, spawnPos, Quaternion.identity);

        Bait bait = baitObject.GetComponent<Bait>();

        if (bait != null)
        {
            bait.SetHealth(playersHealth);
        }
    }
}