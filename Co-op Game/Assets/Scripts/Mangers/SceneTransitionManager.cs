using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    public float transitionInDuration;

    private static bool _enterGame = false;
    private Animator _animator = new();

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
            Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeAnimators();
        PlayTransitionOut();
    }

    private void Start()
    {
        InitializeAnimators();
        _enterGame = true;
    }
    
    private void InitializeAnimators()
    {
        _animator = GetComponentInChildren<Animator>();
        Debug.Log(_animator);
    }

    public void LoadScene()
    {
        PlayTransitionIn();
    }
    
    
    private void PlayTransitionIn()
    {
        Debug.Log("cheese");
        _animator.Play("TransitionIn", 0, 0f);
        
    }

    private void PlayTransitionOut()
    {
        if (!_enterGame) return;
        _animator.Update(0f);

        _animator.Play("TransitionOut", 0, .35f);
        
        
    }
    
}