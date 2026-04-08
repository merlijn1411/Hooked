using System;
using System.Collections;
using UnityEngine;

public class HookWarningIndecator : MonoBehaviour
{
    [Header("Indicator Settings")]
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private float warningTime = 2f;
    [SerializeField] private float groundY = -1.29f;

    private GameObject _indicatorInstance;
    private float _lastIndicatorX = float.NaN;

    // Instantiate the indicator and wait the warning time.
    // Do NOT destroy the indicator here: the owner (HookRandomizer) will remove it when the hook actually drops.
    public IEnumerator ShowWarning(Action onReady)
    {       
        if (indicatorPrefab != null)
        {
            if (_indicatorInstance == null)
            {
                _indicatorInstance = Instantiate(indicatorPrefab);
            }

            float startX = transform.position.x;
            _lastIndicatorX = startX;
            _indicatorInstance.transform.position = new Vector3(startX, groundY, 0f);
        }

        yield return new WaitForSeconds(warningTime);

        // Notify owner that warning time elapsed and hook may attempt to drop (owner still controls slots)
        onReady?.Invoke();
    }

    // Called by owner when the hook actually starts going down (or to cancel the indicator).
    public void HideIndicator()
    {
        if (_indicatorInstance != null)
        {
            Destroy(_indicatorInstance);
            _indicatorInstance = null;
            _lastIndicatorX = float.NaN;
        }
    }

    // Owner pushes updates after it moves.
    public void RefreshIndicatorAt(float worldX)
    {
        if (_indicatorInstance == null) return;

        // Only update when X changed meaningfully to avoid unnecessary work.
        if (float.IsNaN(_lastIndicatorX) || Mathf.Abs(worldX - _lastIndicatorX) > 0.001f)
        {
            _lastIndicatorX = worldX;
            _indicatorInstance.transform.position = new Vector3(worldX, groundY, 0f);
        }
    }
}