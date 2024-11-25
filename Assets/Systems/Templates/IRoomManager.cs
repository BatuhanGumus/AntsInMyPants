using System;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public interface IRoomManager
{ 
    Dictionary<string, RoomInfo> CachedRoomList { get; }
    public event Action OnCachedRoomListUpdated;

    void CreateRoom(string roomName);
    void JoinRoom(string roomName);
    void LeaveRoom();

    Dictionary<Player, GamePlayer> Players { get; }
    GamePlayer LocalPlayer { get; }
    void RegisterSpawnedPlayer(Player player, GamePlayer gamePlayer);
}