using System.IO;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    [SerializeField] private GameFile _saveGameFile = new GameFile();
    
    public static FileManager Instance;
    private void Awake()
    {
        Instance = this;
        Refresh();
    }

    public void Refresh()
    {
        _saveGameFile = Load();
    }

    /// <summary>
    /// Writing the file with existing objects or creating new ones, argument level is between 0 and 1. Only use 1 when you have completed an new level.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="playerCharacter"></param>
    /// <param name="level"></param>
    public void WriteFile(int key, ExternalVariables playerCharacter, int level)
    {
        _saveGameFile = Load();
        _saveGameFile.Add($"Player{key}", playerCharacter, level);
        SaveToJson(_saveGameFile);
    }
    
    public static void SaveToJson(GameFile gameFile)
    {
        var pad = Path.Combine(Application.persistentDataPath, "data.json");
        
        var json = JsonUtility.ToJson(gameFile, true); 
        
        File.WriteAllText(pad, json);

        Debug.Log("Opgeslagen op: " + pad);
    }
    
    public static GameFile Load()
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
