using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    
    private FileManager _fileManager;
    private GameFile _saveGameFile = new GameFile();
    private void Start()
    {
        _fileManager = FileManager.Instance;
        
        _fileManager.Refresh();
        
        LoadPlayers();
    }

    private void LoadPlayers()
    {
        _saveGameFile = _fileManager.Load();
        var players = _saveGameFile.ImportantFiles;

        foreach (var player in players)
        {
            playerManager.SpawnPlayer(player);
        }
    }
}
