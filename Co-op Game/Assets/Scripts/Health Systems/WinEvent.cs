using System;
using UnityEngine;

public class WinEvent : MonoBehaviour
{
    public static event Action OnPlayersWon;

    public static void TriggerWin()
    {
        OnPlayersWon?.Invoke();
    }
}
