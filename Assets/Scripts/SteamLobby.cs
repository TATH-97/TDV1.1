using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;

public class SteamLobby : MonoBehaviour
{

    public static SteamLobby instance;

    //callbacks
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;

    //vars
    public ulong currentLobbyID;
    private const string hostAddressKey="HostAddress";
    private CustomNetWorkManager manager;

    //GameObjects
<<<<<<< Updated upstream
=======
    public GameObject HostButton;
    //public GameObject StartButton;
    public Text lobbyNameText;
>>>>>>> Stashed changes

    private void Start() {
        if(!SteamManager.Initialized) {
            Debug.Log("Steam isnt on");
            return;
        }
        if(instance==null) {
            instance=this;
        }
        manager=GetComponent<CustomNetWorkManager>();
        lobbyCreated=Callback<LobbyCreated_t>.Create(onLobbyCreated);
        JoinRequest=Callback<GameLobbyJoinRequested_t>.Create(onJoinRequest);
        LobbyEntered=Callback<LobbyEnter_t>.Create(onLobbyEntered);
    }
    public void hostLobby() {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, manager.maxConnections);
    }

    //functions 
    private void onLobbyCreated(LobbyCreated_t callback) {
        if(callback.m_eResult !=EResult.k_EResultOK) {
            return;
        }
        Debug.Log("Lobby Created");
        manager.StartHost();
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), hostAddressKey, SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", SteamFriends.GetPersonaName().ToString()+"'s Lobby");
    }

    private void onJoinRequest(GameLobbyJoinRequested_t callback) {
        Debug.Log("request to join lobby");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void onLobbyEntered(LobbyEnter_t callback) {
        //everyone
<<<<<<< Updated upstream
=======
        HostButton.SetActive(false);
        //StartButton.SetActive(true);
>>>>>>> Stashed changes
        currentLobbyID=callback.m_ulSteamIDLobby;

        //Clients
        if(NetworkServer.active) {
            return;
        }
        manager.networkAddress=SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), hostAddressKey);
        manager.StartClient();
    }
}
