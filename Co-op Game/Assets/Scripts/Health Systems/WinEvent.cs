using System;
using UnityEngine;

public class WinEvent : MonoBehaviour
{
    [SerializeField] private int UnlockingNextLevel;
    public static event Action OnPlayersWon;
    
    
    public void TriggerWin()
    {
        FileManager.Instance.WriteUnlockedLevels(UnlockingNextLevel);
        OnPlayersWon?.Invoke();
    }
}
