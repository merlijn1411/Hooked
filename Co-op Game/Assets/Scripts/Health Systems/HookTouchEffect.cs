using UnityEngine;

public class HookTouchEffect : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private SnapMechanic snapMechanic;

    [Header("Effects")]
    [SerializeField] private ParticleSystem hitEffect;

    [Header("Audio Clip")]
    [SerializeField] private AudioClip hookDamageClip;

    private PlayersHealth playersHealth;

    private void Awake()
    {
        playersHealth = FindAnyObjectByType<PlayersHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        Instantiate(hitEffect, transform.position, Quaternion.Euler(-90f, 0f, 0f));
        SoundManager.Instance.PlaySoundFXClip(hookDamageClip, transform, 1f);
        playersHealth.TakingDamage();

        if (!playersHealth.HasLives())
        {
            StartCoroutine(snapMechanic.Snap(collision.transform));
        }
    }

    public void SetHealth(PlayersHealth pHealth)
    {
        playersHealth = pHealth;
    }
}
