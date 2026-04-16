using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Change Cycle settings")]
    [Tooltip("The Seconds how long this timer is.")][SerializeField] private float cycleDuration;

    [SerializeField] private Image filledImage;
    private float _timer;

    private void Start()
    {
        _timer = cycleDuration;
        StartCoroutine(UpdateScroller());
    }

    private IEnumerator UpdateScroller()
    {
        yield return new WaitForSeconds(0);
        _timer -= Time.deltaTime;
        Debug.Log(_timer);
        if (_timer < 0)
        {
            StopCoroutine(UpdateScroller());
            WinEvent.TriggerWin();
        }

        if (!(_timer > 0)) yield break;
        var currentTime = _timer / cycleDuration;
        filledImage.fillAmount = currentTime;
        yield return StartCoroutine(UpdateScroller());

    }
}
