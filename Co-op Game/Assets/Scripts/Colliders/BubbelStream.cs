using UnityEngine;

public class BubbelStream : MonoBehaviour
{
    [SerializeField] float bubbleForce = 5f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var rb = other.GetComponent<Rigidbody2D>();
        rb.velocity += Vector2.up * bubbleForce * Time.deltaTime;
    }
}
