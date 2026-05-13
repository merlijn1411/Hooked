using System.Collections;
using UnityEngine;

public class HookDropManager : MonoBehaviour
{
    public static HookDropManager Instance;

    private int _currentDrops;

    private int minDrops = 1;
    private int maxDrops = 3;

    private int _currentMaxDrops;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _currentMaxDrops = Random.Range(minDrops, maxDrops + 1);
    }

    public IEnumerator RequestDrop()
    {
        while (_currentDrops >= _currentMaxDrops)
            yield return null;

        _currentDrops++;
    }

    public void DropFinished()
    {
        _currentDrops = Mathf.Max(0, _currentDrops - 1);
        _currentMaxDrops = Random.Range(minDrops, maxDrops + 1);
    }
}