using System.IO;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private GameFile _saveGameFile;
    private void Awake()
    {
        SaveToJson(_saveGameFile);
        var file = Load();
        Debug.Log(file);
    }
    
    private static void SaveToJson(GameFile gameFile)
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
