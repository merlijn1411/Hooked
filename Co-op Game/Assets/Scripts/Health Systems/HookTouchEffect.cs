using UnityEngine;

public class HookTouchEffect : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayersHealth playersHealth;

    [SerializeField] private SnapMechanic snapMechanic;

    [Header("Effects")]
    [SerializeField] private ParticleSystem hitEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return; 
        Instantiate(hitEffect, transform.position, Quaternion.Euler(-90f, 0f, 0f));
        playersHealth.TakingDamage();

        if (!playersHealth.HasLives())
        {
            StartCoroutine(snapMechanic.Snap(collision.transform));
        }
    }
}
