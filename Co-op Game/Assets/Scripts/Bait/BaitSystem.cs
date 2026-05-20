using UnityEngine;

public class BaitSystem : MonoBehaviour
{
    public enum Mode { Spawner, Bait }

    [Header("Mode")]
    [SerializeField] private Mode mode;

    [Header("Prefab (alleen Spawner)")]
    [SerializeField] private GameObject baitPrefab;

    [Header("Health Reference (DRAG JE HEALTH UI HIERIN)")]
    [SerializeField] private PlayersHealth playersHealth;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnRate = 3f;
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float spawnY = 6f;

    [Header("Bait Settings")]
    [SerializeField] private float fallSpeed = 2f;
    [SerializeField] private float destroyY = -6f;

    private void Start()
    {
        if (mode == Mode.Spawner)
        {
            InvokeRepeating(nameof(SpawnBait), 1f, spawnRate);
        }
    }

    private void Update()
    {
        if (mode != Mode.Bait) return;

        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }

    private void SpawnBait()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);

        GameObject bait = Instantiate(baitPrefab, spawnPos, Quaternion.identity);

        BaitSystem baitScript = bait.GetComponent<BaitSystem>();
        baitScript.mode = Mode.Bait;

        // BELANGRIJK: geef health reference door
        baitScript.playersHealth = playersHealth;
        baitScript.fallSpeed = fallSpeed;
        baitScript.destroyY = destroyY;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (mode != Mode.Bait) return;

        if (!other.CompareTag("Player")) return;

        if (playersHealth != null)
        {
            playersHealth.AddHeart();
        }

        Destroy(gameObject);
    }
}