using UnityEngine;
using WebSocketSharp;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Linq;

public class PhoneInputManager : MonoBehaviour
{
    [Header("Debug Settings")]
    [SerializeField] private bool enableDebugLogs = true;

    [SerializeField] private ServerListener serverListener;

    private WebSocket _ws;
    private ConcurrentQueue<string> _messageQueue = new ConcurrentQueue<string>();
    
    private PlayerManager _playerManager;

    private void Start()
    {
        var ip = GetLocalIPv4();
        var serverURL = $"ws://{ip}:3000";

        _ws = new WebSocket(serverURL);
        _playerManager = GetComponent<PlayerManager>();
        
        _ws.OnOpen += (sender, e) =>
        {
            if (enableDebugLogs) Debug.Log("✅ Connected to server!");
            _ws.Send("{\"type\":\"register\",\"clientType\":\"unity\"}");
        };

        _ws.OnMessage += (sender, e) =>
        {
            if (enableDebugLogs) Debug.Log("📩 Input received from phone: " + e.Data);
            _messageQueue.Enqueue(e.Data);
        };

        _ws.OnClose += (sender, e) =>
        {
            ServerListener.EndServer();
            if (enableDebugLogs) Debug.LogWarning("❌ Disconnected from server. Reason: " + e.Reason);
        };

        _ws.OnError += (sender, e) =>
        {
            if (enableDebugLogs) Debug.LogError("⚠️ WebSocket Error: " + e.Message);
        };

        _ws.ConnectAsync();
    }
    
    private static string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName())
            .AddressList
            .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)
            ?.ToString();
    }

    void Update()
    {
        while (_messageQueue.TryDequeue(out string command))
        {
            InputMessage msg = JsonUtility.FromJson<InputMessage>(command);

            if (msg != null && msg.type == "input")
            {
                if (enableDebugLogs)
                    Debug.Log($"🎮 Player {msg.playerId} did {msg.action} (x: {msg.x}, y: {msg.y})");

                // We geven nu ook x en y mee aan de PlayerManager!
                _playerManager.HandlePlayerInput(msg.playerId, msg.action, msg.x, msg.y);
            }
            else if (msg != null && msg.type == "disconnect")
            {
                if (enableDebugLogs)
                    Debug.Log($"❌ Player disconnected: {msg.playerId}");

                _playerManager.RemovePlayer(msg.playerId);
            }
        }
    }
    
    private void OnApplicationQuit()
    {
        if (_ws != null)
        {
            _ws.Close();
        }
    }

    private void OnEnable()
    {
        serverListener.EnableLogs(enableDebugLogs);
    }
}