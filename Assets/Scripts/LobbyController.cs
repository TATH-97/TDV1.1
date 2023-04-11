using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;
using System.Linq;

public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;

    //UI stuff
    public Text lobbyNameText;

    //player Data
    public GameObject playerListViewContent;
    public GameObject playerListItemPrefab;
    public GameObject localPlayerObject;

    //other data
    public ulong currentLobbyID;
    public bool playerItemCreated =false;
    private List<PlayerListItem> playerListItems =new List<PlayerListItem>();
    public PlayerObjectController localPlayerController;

    //Manager
    private CustomNetWorkManager manager;
    private CustomNetWorkManager Manager {
        get {
            if(manager!=null) {
                return manager;
            }
            return manager=CustomNetWorkManager.singleton as CustomNetWorkManager;
        }
    }

    private void Awake() {
        if(instance==null) {
            instance=this;
        }
    }

    public void updateLobbyName() {
        currentLobbyID=Manager.GetComponent<SteamLobby>().currentLobbyID;
        lobbyNameText.text=SteamMatchmaking.GetLobbyData(new CSteamID(currentLobbyID), "name");
        Debug.Log("lobby name="+lobbyNameText.text);
    }

    public void updatePlayerList() {
        if(!playerItemCreated) {
            createHostPlayerItem(); //host
        }
        if(playerListItems.Count<Manager.gamePlayers.Count) {
            createClientPlayerItem();
        }
        if(playerListItems.Count>Manager.gamePlayers.Count) {
            removePlayerItem();
        }
        if(playerListItems.Count==Manager.gamePlayers.Count) {
            updatePlayerItem();
        }
    }

    public void findLocalPlayer() {
        localPlayerObject =GameObject.Find("LocalGamePlayer");
        //localPlayerController=localPlayerObject.GetComponent<PlayerObjectController>;
    }

    public void createHostPlayerItem() {
        foreach(PlayerObjectController player in Manager.gamePlayers) {
            GameObject newPlayerItem=Instantiate(playerListItemPrefab) as GameObject;
            PlayerListItem newPlayerItemScript=newPlayerItem.GetComponent<PlayerListItem>();

            newPlayerItemScript.playerName=player.playerName;
            newPlayerItemScript.connectionID=player.connectionID;
            newPlayerItemScript.playerSteamID=player.playerSteamID;
            newPlayerItemScript.setPlayerValues();

            newPlayerItem.transform.SetParent(playerListViewContent.transform);
            //may need to swap to Vect3
            newPlayerItem.transform.localScale=Vector2.one;
            playerListItems.Add(newPlayerItemScript);
        }
        playerItemCreated=true;
    }

    public void createClientPlayerItem() {
        foreach(PlayerObjectController player in Manager.gamePlayers) {
            if(!playerListItems.Any(b => b.connectionID==player.connectionID)) {
                GameObject newPlayerItem=Instantiate(playerListItemPrefab) as GameObject;
                PlayerListItem newPlayerItemScript=newPlayerItem.GetComponent<PlayerListItem>();

                newPlayerItemScript.playerName=player.playerName;
                newPlayerItemScript.connectionID=player.connectionID;
                newPlayerItemScript.playerSteamID=player.playerSteamID;
                newPlayerItemScript.setPlayerValues();

                newPlayerItem.transform.SetParent(playerListViewContent.transform);
                //may need to swap to Vect3
                newPlayerItem.transform.localScale=Vector2.one;
                playerListItems.Add(newPlayerItemScript);
            }
        }

    }

    public void updatePlayerItem() {
        foreach(PlayerObjectController player in Manager.gamePlayers) {
            foreach(PlayerListItem  playerListItemScript in playerListItems) {
                if(playerListItemScript.connectionID==player.connectionID) {
                    playerListItemScript.playerName=player.playerName;
                    playerListItemScript.setPlayerValues();
                }
            }
        }
    }

    public void removePlayerItem() {
        List<PlayerListItem> playerListItemToRemove = new List<PlayerListItem>();

        foreach(PlayerListItem playerListItem in playerListItems) {
            if(!Manager.gamePlayers.Any(b => b.connectionID==playerListItem.connectionID)) {
                playerListItemToRemove.Add(playerListItem);
            }
        }
        if(playerListItemToRemove.Count>0) {
            foreach(PlayerListItem playerlistItemToRemove in playerListItemToRemove) {
                GameObject objectToRemove =playerlistItemToRemove.gameObject;
                playerListItems.Remove(playerlistItemToRemove);
                Destroy(objectToRemove);
                objectToRemove=null;
            }
        }
    }
    

}
