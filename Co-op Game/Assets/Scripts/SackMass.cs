using UnityEngine;

public class SackMass : MonoBehaviour
{
    private LobbyManager _lobbyManager;
    private Rigidbody2D _rb2D;
    
    private void Start()
    {
        _lobbyManager = LobbyManager.Instance;
        _rb2D = GetComponent<Rigidbody2D>();
        _rb2D.linearDamping = 12.25f;

        CalculateMass();
    }
    

    private void CalculateMass()
    {
        // var damping = PlayerCount > 0 ? 10 + (1.125f * PlayerCount) : 10f;
        var playerCount = _lobbyManager.players.Count;
        switch (playerCount)
        {
            case 1:
                _rb2D.linearDamping = 10f;
                break;
            case 2:
                _rb2D.linearDamping = 11.125f;
                break;
            case 3:
                _rb2D.linearDamping = 12.25f;
                break;
        }
    }
    
}
