using UnityEngine;
using WebSocketSharp;
using System.Collections.Concurrent;

public class PhoneInputManager : MonoBehaviour
{
    [Header("Debug Settings")]
    public bool enableDebugLogs = true;

    private WebSocket ws;

    private ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();

    public PlayerManager playerManager;

    public InputMessage InputMessage;

    // TODO: Add a reference to your player script here, e.g.:
    // public PlayerController player;

    void Start()
    {
        string serverIP = "192.168.68.131";
        string serverURL = $"ws://{serverIP}:3000";

        ws = new WebSocket(serverURL);

        ws.OnOpen += (sender, e) =>
        {
            if (enableDebugLogs) Debug.Log("✅ Connected to server!");
            ws.Send("{\"type\":\"register\",\"clientType\":\"unity\"}");
        };

        ws.OnMessage += (sender, e) =>
        {
            if (enableDebugLogs) Debug.Log("📩 Input received from phone: " + e.Data);

            messageQueue.Enqueue(e.Data);
        };

        ws.OnClose += (sender, e) =>
        {
            if (enableDebugLogs) Debug.LogWarning("❌ Disconnected from server. Reason: " + e.Reason);
        };

        ws.OnError += (sender, e) =>
        {
            if (enableDebugLogs) Debug.LogError("⚠️ WebSocket Error: " + e.Message);
        };

        ws.ConnectAsync(); // Asynchroon verbinden (voorkomt vastlopen van Unity tijdens verbinden)
    }

    void Update()
    {
        // Dequeue items frame by frame on the main thread where it is safe to interact with Unity components
        while (messageQueue.TryDequeue(out string command))
        {
            if (enableDebugLogs) Debug.Log("Processing: " + command);

            InputMessage msg = JsonUtility.FromJson<InputMessage>(command);

            if (msg != null && msg.type == "input")
            {
                if (enableDebugLogs)
                    Debug.Log($"🎮 Player {msg.playerId} did {msg.action}");

                playerManager.HandlePlayerInput(msg.playerId, msg.action);
            }
            else if (msg.type == "disconnect")
            {
                if (enableDebugLogs)
                    Debug.Log($"❌ Player disconnected: {msg.playerId}");

                playerManager.RemovePlayer(msg.playerId);
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (ws != null)
        {
            ws.Close();
        }
    }
}