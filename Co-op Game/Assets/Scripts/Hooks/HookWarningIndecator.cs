using UnityEngine;
using System.Collections;

public class HookWarningIndecator : MonoBehaviour
{
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private float groundY = -1.29f;

    private GameObject _indicatorInstance;

    public IEnumerator ShowWarning(float duration)
    {
        if (indicatorPrefab != null)
        {
            _indicatorInstance = Instantiate(indicatorPrefab);
            _indicatorInstance.transform.position = new Vector3(
                transform.position.x,
                groundY,
                0
            );
        }

        yield return new WaitForSeconds(duration);

        HideIndicator();
    }

    public void RefreshIndicatorAt(float xPos)
    {
        if (_indicatorInstance != null)
        {
            _indicatorInstance.transform.position = new Vector3(
                xPos,
                groundY,
                0
            );
        }
    }

    public void HideIndicator()
    {
        if (_indicatorInstance != null)
            Destroy(_indicatorInstance);
    }
}