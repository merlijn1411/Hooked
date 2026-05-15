using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitioner : MonoBehaviour
{
    [SerializeField] private List<GameObject> parentHolders;
    private List<Animator> _animators = new List<Animator>();

    private void Start()
    {
        InitializeAnimators();
    }
    
    private void InitializeAnimators()
    {
        foreach (var holder in parentHolders)
        {
            var childObjects = holder.GetComponentsInChildren<Animator>();
            foreach (var childObject in childObjects)
            {
                _animators.Add(childObject);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayTransition();
        }
        
    }

    private void PlayTransition()
    {
        foreach (var animator in _animators)
        {
           animator.SetTrigger("Play");
        }
    }
}
