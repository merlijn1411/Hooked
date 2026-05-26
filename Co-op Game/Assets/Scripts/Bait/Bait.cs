using UnityEngine;

public class Bait : MonoBehaviour
{
    [Header("Bait Settings")]
    [SerializeField] private float _fallSpeed = 2f;
    [SerializeField] private float _destroyY = -5f;

    private PlayersHealth _playersHealth;

    public void SetHealth(PlayersHealth health)
    {
        _playersHealth = health;
    }

    private void Update()
    {
        transform.position += Vector3.down * _fallSpeed * Time.deltaTime;

        if (transform.position.y < _destroyY)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (_playersHealth != null)
            _playersHealth.AddHeart();

        Destroy(gameObject);
    }
}