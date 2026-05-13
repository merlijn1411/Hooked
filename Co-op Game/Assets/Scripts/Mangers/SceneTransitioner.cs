using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitioner : MonoBehaviour
{
    [SerializeField] private List<Animator> animators;

    private void Start()
    {
        PlayTransition();
    }


    private void PlayTransition()
    {
        foreach (var animator in animators)
        {
           animator.SetTrigger("Play");
        }
    }
}
