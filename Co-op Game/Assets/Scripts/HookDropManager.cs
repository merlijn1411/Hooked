using System.Collections;
using UnityEngine;

public class HookDropManager : MonoBehaviour
{
    public static HookDropManager Instance;

    private int _currentDrops = 0;
    private const int _maxDrops = 2;

    void Awake()
    {
        Instance = this;
    }

    public IEnumerator RequestDrop()
    {
        while (_currentDrops >= _maxDrops)
            yield return null;

        _currentDrops++;
    }

    public void DropFinished()
    {
        _currentDrops = Mathf.Max(0, _currentDrops - 1);
    }
}