using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class PlayerObjectController : NetworkBehaviour
{
   //Player Data
   [SyncVar] public int connectionID;
   [SyncVar] public int playerIDNumber;
   [SyncVar] public ulong playerSteamID;
   [SyncVar(hook =nameof(playerNameUpdate))] public string playerName;


    private CustomNetWorkManager manager;
    private CustomNetWorkManager Manager {
        get {
            if(manager!=null) {
                return manager;
            }
            return manager=CustomNetWorkManager.singleton as CustomNetWorkManager;
        }
    }

    public override void OnStartAuthority() {
        Debug.Log("OnStartAuthority");
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name="LocalGamePlayer";
        LobbyController.instance.findLocalPlayer();
        LobbyController.instance.updateLobbyName();
    }

    public override void OnStartClient() {
        Debug.Log("OnStartClient");
        Manager.gamePlayers.Add(this);
        LobbyController.instance.updateLobbyName();
        LobbyController.instance.updatePlayerList();
    }

    public override void OnStopClient() {
        Manager.gamePlayers.Remove(this);
        LobbyController.instance.updatePlayerList();
    }

    [Command]
    private void CmdSetPlayerName(string playerName) {
        this.playerNameUpdate(this.playerName, playerName);
    }


    public void playerNameUpdate(string oldValue, string newValue) {
        if(isServer) { //host
            this.playerName=newValue;
        }
        if(isClient) { //client
            LobbyController.instance.updatePlayerList();
        }
    }

}
