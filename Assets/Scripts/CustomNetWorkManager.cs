using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using Steamworks;

public class CustomNetWorkManager : NetworkManager
{
    [SerializeField]private PlayerObjectController gamePlayerPrefab;
    public List<PlayerObjectController> gamePlayers {get; }=new List<PlayerObjectController>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn) {
        if(SceneManager.GetActiveScene().name=="lobby") {
            PlayerObjectController gamePlayerInstance=Instantiate(gamePlayerPrefab);
            gamePlayerInstance.connectionID=conn.connectionId;
            gamePlayerInstance.playerIDNumber=gamePlayers.Count+1;
            gamePlayerInstance.playerSteamID=(ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.currentLobbyID, gamePlayers.Count);
            
            NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject);
        }
    }

}
