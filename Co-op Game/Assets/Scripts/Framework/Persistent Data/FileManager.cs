using System.IO;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    public static FileManager Instance;
    
    [SerializeField] private GameFile saveGameFile = new GameFile();
    [SerializeField] private bool isLobby;
    
    // Deze variabele wordt alleen bij een compleet nieuwe opstart van de game op 'true' gezet
    private static bool _isFirstBoot = true;
    
    private void Awake()
    {
        Instance = this;
        saveGameFile = Load();
    }

    private void Start()
    {
        if (_isFirstBoot)
        {
            RemoveAllPlayers();
            _isFirstBoot = false; 
        }
    }
    
    /// <summary>
    /// Writing the file with existing objects or creating new ones
    /// </summary>
    /// <param name="key"></param>
    /// <param name="playerCharacter"></param>
    public void WritePlayer(int key, ExternalVariables playerCharacter)
    {
        saveGameFile = Load();
        saveGameFile.AddPlayer($"Player {key}", playerCharacter);
        SaveToJson(saveGameFile);
    }

    public void RemovePlayer(string key)
    {
        saveGameFile = Load();
        saveGameFile.DeletePlayer(key);
    }
    

    private void RemoveAllPlayers()
    {
        saveGameFile = Load();
        saveGameFile.DeleteAllPlayers();
        SaveToJson(saveGameFile);
    }
    
    /// <summary>
    /// Updates the current amount of unlocked levels, to increase the number type "Positive". To Decrease the number type anything but "Positive"
    /// </summary>
    /// <param name="status"></param>
    public void WriteUnlockedLevels(int level)
    {
        saveGameFile = Load();
        saveGameFile.UnlockedLevel = level;
        Debug.Log("Hey");
        SaveToJson(saveGameFile);
    }
    
    private static void SaveToJson(GameFile gameFile)
    {
        var pad = Path.Combine(Application.persistentDataPath, "data.json");
        
        var json = JsonUtility.ToJson(gameFile, true); 
        
        File.WriteAllText(pad, json);

        //Debug.Log("Opgeslagen op: " + pad);
    }
    
    public GameFile Load()
    {
        var path = Path.Combine(Application.persistentDataPath, "data.json");

        var doesExist = File.Exists(path) ? GetFile(path) : new GameFile();
        var doesExistMess = File.Exists(path) ? "File is getting loaded" : "No SaveFile found, create new one";
        
        Debug.Log(doesExistMess);
        return doesExist;
    }

    private static GameFile GetFile(string pad)
    {
        var json = File.ReadAllText(pad);
            
        return JsonUtility.FromJson<GameFile>(json);
    }

    public static void DeleteJson()
    {
        var path = Path.Combine(Application.persistentDataPath, "data.json");
        if (File.Exists(path)) File.Delete(path);
    }
}
