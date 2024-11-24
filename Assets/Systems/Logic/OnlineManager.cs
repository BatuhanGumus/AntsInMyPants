using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class OnlineManager : MonoBehaviourPunCallbacks, IOnlineManager
{
    public ClientState OnlineState
    {
        get
        {
            return PhotonNetwork.NetworkClientState;
        }
    }
    
    private void Awake()
    {
        if (GameData.OnlineManager == null)
        {
            GameData.OnlineManager = this;
        }
        else
        {
            Debug.LogError("[Online Manager]: two instances of the game flow, destroying newest");
            Destroy(gameObject);
        }
    }

    public void Login()
    {
        string playerName = GameData.SaveManager.CurrentSave.username;

        if (!playerName.Equals(""))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogError("[Online Manager]: Username null or empty");
        }
    }
    
    // Called when login successful
    public override void OnConnectedToMaster()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
}