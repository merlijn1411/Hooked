using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ServerListener : MonoBehaviour
{
    [SerializeField] private PhoneInputManager phoneInputManager;

    private static bool _enableDebugLogs;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void StartSever()
    {
        var localizedFiled = LocalizePaths("start-server.bat");
        
        var psi = new ProcessStartInfo()
        {
            FileName = localizedFiled[0],
            WorkingDirectory = $"{localizedFiled[1]}",
            UseShellExecute = _enableDebugLogs, 
            CreateNoWindow = false
        };
        
        try {
            Process.Start(psi);
        }
        catch (System.Exception e) {
            Debug.LogError("Kon batch niet starten: " + e.Message);
        }
    }
    
    public void EnableLogs(bool isEnabled)
    { 
        _enableDebugLogs = isEnabled;
    } 

    public static void EndServer()
    {
        var localizedFiled = LocalizePaths("end-server.bat");
        
        var psi = new ProcessStartInfo()
        {
            FileName = localizedFiled[0],
            WorkingDirectory = $"{localizedFiled[1]}",
            UseShellExecute = false, 
            CreateNoWindow = false
        };
        
        try {
            Process.Start(psi);
        }
        catch (System.Exception e) {
            Debug.LogError("Kon batch niet afsluiten: " + e.Message);
        }
    }

    private static List<string> LocalizePaths(string filename)
    {
        var projectRoot = Path.GetDirectoryName(Application.dataPath);
        var folderPath = Path.Combine(projectRoot, "Multiplayer Servers");
        
        var fullPathToBat = Path.Combine(folderPath, filename);
        
        var pathList = new List<string>
        {
            fullPathToBat,
            folderPath
        };
        return pathList;
    }
}
