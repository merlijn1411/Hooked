using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Change Cycle settings")]
    [Tooltip("The Seconds how long this timer is.")][SerializeField] private float cycleDuration;

    [Header("Images")]
    [SerializeField] private Image filledImage;

    [Header("Animator")]
    [SerializeField] private Animator winLoseAnimator;

    [Header("Audio Clip")]
    [SerializeField] private AudioClip wonSound;


    private float _timer;
    private bool _hasTriggered = false;

    private void Start()
    {
        winLoseAnimator.enabled = false;
        _timer = cycleDuration;
    }

    private void Update()
    {
        UpdateScroller();
    }

    private void UpdateScroller()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0 && !_hasTriggered)
        {
            _hasTriggered = true;
            winLoseAnimator.enabled = true;
            SoundManager.Instance.PlaySoundFXClip(wonSound, transform, 1f);
            WinEvent.TriggerWin();
            return;
        }

        if (!(_timer > 0)) return;
        var currentTime = _timer / cycleDuration;
        filledImage.fillAmount = currentTime;
    }
}
