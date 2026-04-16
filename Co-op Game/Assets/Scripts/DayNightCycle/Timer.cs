using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Change Cycle settings")]
    [Tooltip("The Seconds how long this timer is.")][SerializeField] private float cycleDuration;

    [SerializeField] private Image filledImage;
    private float _timer;
    private bool _hasTriggered = false;

    private void Start()
    {
        _timer = cycleDuration;
    }

    private void Update()
    {
        UpdateScroller();
    }

    private void UpdateScroller()
    {
        _timer -= Time.deltaTime;
        Debug.Log(_timer);
        if (_timer < 0 && !_hasTriggered)
        {
            _hasTriggered = true;
            WinEvent.TriggerWin();
            return;
        }

        if (!(_timer > 0)) return;
        var currentTime = _timer / cycleDuration;
        filledImage.fillAmount = currentTime;
    }
}
