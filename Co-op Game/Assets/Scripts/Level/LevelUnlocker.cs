using System.Collections.Generic;
using UnityEngine;

public class LevelUnlocker : MonoBehaviour
{
    [SerializeField] private List<Level> _levels = new List<Level> { };
    private int setLevel = 1;
    private void Start()
    {
        InitializeLevels();
        CheckCurrentLevel();
    }

    private void InitializeLevels()
    {
        var levels = GetComponentsInChildren<Level>();
        foreach (var level in levels)
        {
            _levels.Add(level);
        }
    }

    private void CheckCurrentLevel()
    {
        var file = FileManager.Instance.Load();
        var unlockedLevels = file.UnlockedLevel;

        foreach (var level in _levels)
        { 
            level.Id = setLevel;
            setLevel += 1;
            level.isLevelUnlocked(unlockedLevels);
        }
    }
}
