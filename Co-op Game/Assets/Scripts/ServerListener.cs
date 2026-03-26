using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ServerListener : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void StartSever()
    {
        var localizedFiled = LocalizePaths("execute-server.bat");
        
        var psi = new ProcessStartInfo()
        {
            FileName = localizedFiled[0],
            WorkingDirectory = localizedFiled[1],
            UseShellExecute = true, 
            CreateNoWindow = false
        };
        
        try {
            Process.Start(psi);
        }
        catch (System.Exception e) {
            Debug.LogError("Kon batch niet starten: " + e.Message);
        }
    }

    private static void EndServer()
    {
        
    }

    private static List<string> LocalizePaths(string filename)
    {
        var projectRoot = Path.GetDirectoryName(Application.dataPath);
        Debug.Log(projectRoot);
        var folderPath = Path.Combine(projectRoot, "Multiplayer Servers");
    
        Debug.Log("Starten van Node script in: " + folderPath);
        var fullPathToBat = Path.Combine(folderPath, filename);
        
        var pathList = new List<string>
        {
            fullPathToBat,
            folderPath
        };
        return pathList;
    }
}
