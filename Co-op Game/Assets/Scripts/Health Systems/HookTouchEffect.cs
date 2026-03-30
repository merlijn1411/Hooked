using UnityEngine;

public class HookTouchEffect : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayersHealth playersHealth;

    [Header("Effects")]
    [SerializeField] private ParticleSystem hitEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Instantiate(hitEffect, transform.position, Quaternion.Euler(-90f,0f,0f));
            playersHealth.TakingDamage();
        }
    }
}
