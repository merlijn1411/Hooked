using System.Collections;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private Vector2 _lastPosition;

    private void FixedUpdate()
    {
        _lastPosition = transform.position;

        var isMoving = _lastPosition.magnitude > 0.1f;
        
        _animator.SetBool("IsMoving", isMoving);
    }

    public void StartHappy()
    {
        StartCoroutine(IsHappy());
    }

    private IEnumerator IsHappy()
    {
        _animator.SetBool("IsHappy", true);

        yield return new WaitForSeconds(2f);
        
        _animator.SetBool("IsHappy", false);
    }
    
    public void StartHit()
    {
        StartCoroutine(HasBeenHit());
    }

    private IEnumerator HasBeenHit()
    {
        _animator.SetBool("HasHitted", true);
        
        yield return new WaitForSeconds(2f);
        
        _animator.SetBool("HasHitted", false);
    }
}
