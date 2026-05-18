using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerLoader playerLoader;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CharacterDatabase characterDatabase;
    
    private Dictionary<string, PlayerMovement> players = new Dictionary<string, PlayerMovement>();
    

    // Voeg float x en float y toe aan parameters met een standaardwaarde van 0
    public void HandlePlayerInput(string playerId, string action, float x = 0f, float y = 0f)
    {
        
        var playerMovement = players[playerId];
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

    public void SpawnPlayer(ExternalFiles playerFile)
    {
        var obj = Instantiate(characterDatabase.GetByIndex(playerFile.Value.Index), spawnPoint.position, 
            Quaternion.identity, spawnPoint);
        var playerMovement = obj.GetComponent<PlayerMovement>();
        players.Add(playerFile.Value.ID, playerMovement);
        Debug.Log("✅ Spawned playerMovement: " + playerFile.Value.ID);
    }

    public void RemovePlayer(string playerId)
    {
        if (!players.ContainsKey(playerId)) return;

        var playerMovement = players[playerId];
        Destroy(playerMovement.gameObject);
        players.Remove(playerId);

        Debug.Log("🗑️ Removed playerMovement: " + playerId);
    }
}
