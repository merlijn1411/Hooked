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

    void Start()
    {
        string serverIP = "10.120.20.85";
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

        ws.ConnectAsync();
    }

    void Update()
    {
        while (messageQueue.TryDequeue(out string command))
        {
            InputMessage msg = JsonUtility.FromJson<InputMessage>(command);

            if (msg != null && msg.type == "input")
            {
                if (enableDebugLogs)
                    Debug.Log($"🎮 Player {msg.playerId} did {msg.action} (x: {msg.x}, y: {msg.y})");

                // We geven nu ook x en y mee aan de PlayerManager!
                playerManager.HandlePlayerInput(msg.playerId, msg.action, msg.x, msg.y);
            }
            else if (msg != null && msg.type == "disconnect")
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