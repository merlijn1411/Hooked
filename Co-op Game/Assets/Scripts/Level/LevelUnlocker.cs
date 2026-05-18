using UnityEngine;

public class LevelUnlocker : MonoBehaviour
{
    [SerializeField] private Level[] _levels;
    private int setLevel = 1;
    private void Start()
    {
        CheckCurrentLevel();
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
