using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    public float transitionInDuration;
    
    [SerializeField] private List<GameObject> parentHolders;

    private readonly List<Animator> _animators = new();

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
    }
    
    private void InitializeAnimators()
    {
        _animators.Clear();

        foreach (var holder in parentHolders)
        {
            var childAnimators = holder.GetComponentsInChildren<Animator>();

            foreach (var animator in childAnimators)
            {
                _animators.Add(animator);
            }
        }
    }

    public void LoadScene()
    {
        PlayTransitionIn();
    }
    
    
    private void PlayTransitionIn()
    {
        foreach (var animator in _animators)
        {
            animator.Play("TransitionIn", 0, 0f);
        }
    }

    private void PlayTransitionOut()
    {
        foreach (var animator in _animators)
        {
            animator.Rebind();
            animator.Update(0f);

            animator.Play("TransitionOut", 0, .35f);
        }
        
    }
    
}