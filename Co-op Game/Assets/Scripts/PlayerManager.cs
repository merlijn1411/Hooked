using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;

    private Dictionary<string, Player> players = new Dictionary<string, Player>();

    public void HandlePlayerInput(string playerId, string action)
    {
        if (!players.ContainsKey(playerId))
        {
            SpawnPlayer(playerId);
        }

        Player player = players[playerId];

        if (action == "jump") player.Jump();
        if (action == "left") player.MoveLeft();
        if (action == "right") player.MoveRight();
    }

    void SpawnPlayer(string playerId)
    {
        GameObject obj = Instantiate(playerPrefab, RandomSpawn(), Quaternion.identity);
        Player player = obj.GetComponent<Player>();

        players.Add(playerId, player);

        Debug.Log("✅ Spawned player: " + playerId);
    }

    public void RemovePlayer(string playerId)
    {
        if (!players.ContainsKey(playerId)) return;

        Player player = players[playerId];

        Destroy(player.gameObject);

        players.Remove(playerId);

        Debug.Log("🗑️ Removed player: " + playerId);
    }

    Vector3 RandomSpawn()
    {
        return new Vector3(Random.Range(-4, 4), 0, 0);
    }
}
