using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

internal class NetworkEventManager : INetworkEventManager, IOnEventCallback
{
    public Dictionary<NetworkEventType, Action<object[]>> Events => _events;
    private Dictionary<NetworkEventType, Action<object[]>> _events;
    
    public NetworkEventManager()
    {
        _events = new Dictionary<NetworkEventType, Action<object[]>>();
        
        foreach (NetworkEventType value in Enum.GetValues(typeof(NetworkEventType)))
        {
            _events.Add(value, null);
        }
        
        PhotonNetwork.AddCallbackTarget(this);
    }

    ~NetworkEventManager()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void SendEvent(NetworkEventType type, object[] data, ReceiverGroup receiverGroup = ReceiverGroup.All)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = receiverGroup };
        PhotonNetwork.RaiseEvent((byte)type, data, raiseEventOptions, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (_events.TryGetValue((NetworkEventType)eventCode, out var @event))
        {
            @event?.Invoke((object[])photonEvent.CustomData);
        }
    }
}