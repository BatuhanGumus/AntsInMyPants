using System;
using System.Collections.Generic;
using Photon.Realtime;

public interface IRoomManager
{ 
    Dictionary<string, RoomInfo> CachedRoomList { get; }
    public event Action OnCachedRoomListUpdated;

    void CreateRoom(string roomName);
    void JoinRoom(string roomName);
    void LeaveRoom();
}