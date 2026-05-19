using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    
    private GameFile _saveGameFile = new GameFile();
    private void Start()
    {
        LoadPlayers();
    }
    
    private void LoadPlayers()
    {
        _saveGameFile = FileManager.Instance.Load();
        var players = _saveGameFile.PlayerInfo;

        foreach (var player in players)
        {
            // playerManager.SpawnPlayer(player);
        }
    }
}
