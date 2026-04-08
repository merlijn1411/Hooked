using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private Dictionary<string, PlayerMovement> players = new Dictionary<string, PlayerMovement>();

    // Voeg float x en float y toe aan parameters met een standaardwaarde van 0
    public void HandlePlayerInput(string playerId, string action, float x = 0f, float y = 0f)
    {
        if (!players.ContainsKey(playerId))
        {
            SpawnPlayer(playerId);
        }

        PlayerMovement playerMovement = players[playerId];

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

    void SpawnPlayer(string playerId)
    {
        GameObject obj = Instantiate(playerPrefab, RandomSpawn(), Quaternion.identity);
        PlayerMovement playerMovement = obj.GetComponent<PlayerMovement>();

        players.Add(playerId, playerMovement);
        Debug.Log("✅ Spawned playerMovement: " + playerId);
    }

    public void RemovePlayer(string playerId)
    {
        if (!players.ContainsKey(playerId)) return;

        PlayerMovement playerMovement = players[playerId];
        Destroy(playerMovement.gameObject);
        players.Remove(playerId);

        Debug.Log("🗑️ Removed playerMovement: " + playerId);
    }

    Vector3 RandomSpawn()
    {
        return new Vector3(Random.Range(-4, 4), 0, 0);
    }
}
