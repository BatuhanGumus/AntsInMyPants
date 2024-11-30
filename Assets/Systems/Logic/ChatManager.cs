using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ChatManager : IChatManager
{
    public ChatManager()
    {
        GameData.NetworkEventManager.Events[NetworkEventType.ChatMessageEvent] += ChatMessageEvent;
    }
    
    ~ChatManager()
    {
        GameData.NetworkEventManager.Events[NetworkEventType.ChatMessageEvent] -= ChatMessageEvent;
    }
    
    public Queue<Tuple<Player, string>> ChatHistory => _chatHistory;
    private Queue<Tuple<Player, string>> _chatHistory = new Queue<Tuple<Player, string>>();
    public Action<Player,string> OnNewChatMessage { get; set; }
    public Action<bool> OnChatMode { get; set; }
    
    public void SendChatMessage(string message)
    {
        GameData.NetworkEventManager.SendEvent(NetworkEventType.ChatMessageEvent, 
            new object[]
            {
                GameData.RoomManager.LocalPlayer.OwnerPlayer,
                message,
            });
    }

    public bool InChatMode => _inChatMode;
    private bool _inChatMode;

    private void ChatMessageEvent(object[] data)
    {
        _chatHistory.Enqueue(new Tuple<Player, string>((Player)data[0], (string)data[1]));
        OnNewChatMessage?.Invoke(((Player)data[0]), (string)data[1]);
    }

    public void ChatMode(bool state)
    {
        _inChatMode = state;
        OnChatMode?.Invoke(state);
    }
}
