using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerListener : MonoBehaviour
{
    private PhoneInputManager _phoneInputManager;
    private static bool debug;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private void RunNodeScript()
    {
        _phoneInputManager = GetComponent<PhoneInputManager>();
        debug = _phoneInputManager.enableDebugLogs;
        
        Debug.Log(debug);
        
        var projectRoot = Path.GetDirectoryName(Application.dataPath);
        var folderPath = Path.Combine(projectRoot, "Multiplayer-Servers");
    
        UnityEngine.Debug.Log("Starten van Node script in: " + folderPath);
        var batchFileName = "execute-server.bat"; 
        var fullPathToBat = Path.Combine(folderPath, batchFileName);

        var psi = new ProcessStartInfo()
        {
            FileName = fullPathToBat,
            WorkingDirectory = folderPath,
            UseShellExecute = debug, 
            CreateNoWindow = false
        };
        
        try {
            Process.Start(psi);
        }
        catch (System.Exception e) {
            UnityEngine.Debug.LogError("Kon batch niet starten: " + e.Message);
        }
    }
}
