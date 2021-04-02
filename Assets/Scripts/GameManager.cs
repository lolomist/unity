using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string playerIdPrefix = "Player";

    private static Dictionary<string, PlayerController> players = new Dictionary<string, PlayerController>();

    public static void RegisterPlayer(string netID, PlayerController player)
    {
        string playerId = playerIdPrefix + " " + netID;
        players.Add(playerId, player);
        player.transform.name = playerId;
        Debug.Log(playerId);
    }

    public static void UnregisterPlayer(string playerId)
    {
        players.Remove(playerId);
    }

    public static PlayerController GetPlayer(string playerId)
    {
        return players[playerId];
    }

    public static Dictionary<string, PlayerController> GetPlayers()
    {
        return players;
    }
}
