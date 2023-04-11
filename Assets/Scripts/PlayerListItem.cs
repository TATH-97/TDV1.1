using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class PlayerListItem : MonoBehaviour
{
    public string playerName;
    public int connectionID;
    public ulong playerSteamID;
    private bool avatarReceived;
    public Text playerNameText;
    public RawImage playerIcon;
    protected Callback<AvatarImageLoaded_t> imageLoaded;

    private void Start() {
        imageLoaded=Callback<AvatarImageLoaded_t>.Create(onImageLoaded);
    }

private Texture2D GetSteamImageAsTexture(int iImage)
    {
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);
        if (isValid)
        {
            byte[] image = new byte[width * height * 4];

            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));

            if (isValid)
            {
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }
        avatarReceived = true;
        return texture;
    }

    void getPlayerIcon() {
        int imageID=SteamFriends.GetLargeFriendAvatar((CSteamID)playerSteamID);
        if(imageID==-1) {
            return;
        }
        playerIcon.texture=GetSteamImageAsTexture(imageID);
    }

    public void setPlayerValues() {
        playerNameText.text=playerName;
        if(!avatarReceived) {
            getPlayerIcon();
        }
    }
    
    private void onImageLoaded(AvatarImageLoaded_t callBack) {
        if(callBack.m_steamID.m_SteamID==playerSteamID) {
            playerIcon.texture=GetSteamImageAsTexture(callBack.m_iImage);
        }
        else { //another player 
            return;
        }
    }
}
