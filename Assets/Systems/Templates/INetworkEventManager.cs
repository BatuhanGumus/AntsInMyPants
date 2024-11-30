
using System;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public enum NetworkEventType : byte
{
    PingPong = 1,
    ChatMessageEvent = 2,
    CreatureCaught = 3,
    
}

public interface INetworkEventManager
{
    Dictionary<NetworkEventType, Action<object[]>> Events { get; }
    void SendEvent(NetworkEventType type, object[] data, ReceiverGroup receiverGroup = ReceiverGroup.All);
}