using UnityEngine;

public class BubbelStream : MonoBehaviour
{
    [Header("Force of stream")]
    [SerializeField] float bubbleForce;

    [Header("Before bubbelstream is destroyed")]
    [SerializeField] float DestroyStreamTime;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var rb = other.GetComponent<Rigidbody2D>();
        rb.linearVelocity += Vector2.up * bubbleForce * Time.deltaTime;
        Destroy(gameObject, DestroyStreamTime);
    }
}
