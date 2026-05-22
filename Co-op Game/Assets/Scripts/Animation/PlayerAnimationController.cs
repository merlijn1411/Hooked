using System.Collections;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private Vector2 _lastPosition;

    private void Start()
    {
        WinEvent.OnPlayersWon +=  StartHappy;
        PlayersHealth.OnPlayerHitted += StartHit;
    }

    private void Update()
    {
        var distanceMoved = Vector2.Distance(transform.position, _lastPosition);

        var isMoving = distanceMoved > .01f;
        
        _animator.SetBool("IsMoving", isMoving);

        StartCoroutine(UpdateLastPosition());
    }

    private IEnumerator UpdateLastPosition()
    {
        yield return new WaitForSeconds(1f);
        _lastPosition = transform.position;
    }

    private void StartHappy()
    {
        StartCoroutine(IsHappy());
    }

    private IEnumerator IsHappy()
    {
        _animator.SetTrigger("IsHappy");

        yield return new WaitForSeconds(2f);
        
        _animator.SetTrigger("IsHappy");
    }
    
    private void StartHit()
    {
        StartCoroutine(HasBeenHit());
    }

    private IEnumerator HasBeenHit()
    {
        _animator.SetTrigger("HasHitted");
        
        yield return new WaitForSeconds(2f);
        
        _animator.SetTrigger("HasHitted");
    }
}
