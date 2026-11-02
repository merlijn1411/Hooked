using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private Transform SpawnPoint;
     
    private Dictionary<string, PlayerMovement> _players = new Dictionary<string, PlayerMovement>();

    // Voeg float x en float y toe aan parameters met een standaardwaarde van 0
    public void HandlePlayerInput(string playerId, string action, float x = 0f, float y = 0f)
    {
        if (!_players.ContainsKey(playerId))
        {
            Debug.LogWarning($"{playerId} komt niet overeen met ");
        }

        var playerMovement = _players[playerId];

        // Bestaande acties
        if (action == "Y") playerMovement.InteractieY();
        if (action == "A") playerMovement.InteractieA();
        if (action == "B") playerMovement.InteractieB();

        // Nieuwe actie voor de joystick, we geven x en y door naar de PlayerMovement
        if (action == "joystick")
        {
            playerMovement.MoveWithJoystick(x, y);
        }
    }

    public void SpawnPlayer(ExternalFiles player)
    {
        var playerValue = player.Value;
        var obj = Instantiate(characterDatabase.GetByIndex(playerValue.Index), RandomSpawn(), 
            Quaternion.identity, SpawnPoint);
        var playerMovement = obj.GetComponent<PlayerMovement>();

        _players.Add(playerValue.ID, playerMovement);
        Debug.Log("✅ Spawned playerMovement: " + playerValue.ID);
    }

    public void RemovePlayer(string playerId)
    {
        if (!_players.ContainsKey(playerId)) return;

        var playerMovement = _players[playerId];
        Destroy(playerMovement.gameObject);
        _players.Remove(playerId);

        Debug.Log("🗑️ Removed playerMovement: " + playerId);
    }

    private Vector3 RandomSpawn()
    {
        var randomRange = Random.Range(-.7f, .7f);
        var PosX = SpawnPoint.position.x + randomRange;
        var PosY = SpawnPoint.position.y + randomRange;
        return new Vector3(PosX, PosY + randomRange, 0);
    }
}
