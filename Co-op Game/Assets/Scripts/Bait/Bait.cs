using UnityEngine;

public class Bait : MonoBehaviour
{
    [Header("Bait Settings")]
    [SerializeField] private float fallSpeed = 2f;
    [SerializeField] private float destroyY = -5f;

    private PlayersHealth playersHealth;

    public void SetHealth(PlayersHealth health)
    {
        playersHealth = health;
    }

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y < destroyY)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (playersHealth != null)
            playersHealth.AddHeart();

        Destroy(gameObject);
    }
}