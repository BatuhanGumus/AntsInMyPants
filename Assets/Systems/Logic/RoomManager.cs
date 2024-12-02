using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviourPunCallbacks, IRoomManager
{
    public Dictionary<string, RoomInfo> CachedRoomList => _cachedRoomList;
    private Dictionary<string, RoomInfo> _cachedRoomList = new Dictionary<string, RoomInfo>();
    public event Action OnCachedRoomListUpdated;

    private void Awake()
    {
        if (GameData.RoomManager == null)
        {
            GameData.RoomManager = this;
        }
        else
        {
            Debug.LogError("[Room Manager]: two instances of the game flow, destroying newest");
            Destroy(gameObject);
        }
    }
    
    public void CreateRoom(string roomName)
    {
        roomName = (roomName.Equals(string.Empty)) ? $"{GameData.SaveManager.CurrentSave.username}'s Room " + Random.Range(1000, 10000) : roomName;
        byte maxPlayers = 8;
        RoomOptions options = new RoomOptions {MaxPlayers = maxPlayers, PlayerTtl = 10000 };
        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"[Room Manager]: Create room failed with code[{returnCode}]\n message: {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"[Room Manager]: Join room failed with code[{returnCode}]\n message: {message}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError($"[Room Manager]: Join random room failed with code[{returnCode}]\n message: {message}");
    }
    
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (_cachedRoomList.ContainsKey(info.Name))
                {
                    _cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (_cachedRoomList.ContainsKey(info.Name))
            {
                _cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                _cachedRoomList.Add(info.Name, info);
            }
        }
        
        OnCachedRoomListUpdated?.Invoke();
    }
    
    public void JoinRoom(string roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        PhotonNetwork.JoinRoom(roomName);
    }

    public void LeaveRoom()
    {
        GameData.SaveManager.MarkDirty();

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public Dictionary<Player, GamePlayer> Players => _players;
    private Dictionary<Player, GamePlayer> _players = new Dictionary<Player, GamePlayer>();
    public GamePlayer LocalPlayer => _localPlayer;
    private GamePlayer _localPlayer;
    
    public void RegisterSpawnedPlayer(Player player, GamePlayer gamePlayer)
    {
        _players[player] = gamePlayer;
    }

    public override void OnJoinedRoom()
    {
        var spawned = PhotonNetwork.Instantiate(
            "Player",
            Vector3.zero,
            Quaternion.identity,
            0,
            GetPlayerInstantiateData()).GetComponent<GamePlayer>();

        _localPlayer = spawned;
    }

    private object[] GetPlayerInstantiateData()
    {
        if (GameData.SaveManager.CurrentSave == null)
            return null;

        object[] data = new[]
        {
            GameData.SaveManager.CurrentSave.username,
        };

        return data;
    }

    public override void OnLeftRoom()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _players.Remove(otherPlayer);
    }
    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            
        }
    }
}