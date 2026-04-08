using UnityEngine;
using System.Collections;

public class HookWarningIndecator : MonoBehaviour
{
    [Header("Indicator Settings")]
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private float warningTime = 1f;
    [SerializeField] private float groundY = -1.29f;

    private GameObject _indicatorInstance;

    public IEnumerator ShowWarning(System.Action onComplete)
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

        yield return new WaitForSeconds(warningTime);

        if (_indicatorInstance != null)
            Destroy(_indicatorInstance);


        onComplete?.Invoke();
    }

    private void Update()
    {

        if (_indicatorInstance != null)
        {
            _indicatorInstance.transform.position = new Vector3(
                transform.position.x,
                groundY,
                0
            );
        }
    }
}