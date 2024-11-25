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

        OnNewChatMessage += () =>
        {
            var latestMessage = _chatHistory.Peek();
            Debug.Log($"[Chat Manager]: message from {latestMessage.Item1.NickName} - \"{latestMessage.Item2}\"");
        };
    }
    
    ~ChatManager()
    {
        GameData.NetworkEventManager.Events[NetworkEventType.ChatMessageEvent] -= ChatMessageEvent;
    }
    
    public Queue<Tuple<Player, string>> ChatHistory => _chatHistory;
    private Queue<Tuple<Player, string>> _chatHistory = new Queue<Tuple<Player, string>>();
    public Action OnNewChatMessage { get; }
    public void SendChatMessage(string message)
    {
        GameData.NetworkEventManager.SendEvent(NetworkEventType.ChatMessageEvent, 
            new object[]
            {
                GameData.RoomManager.LocalPlayer.OwnerPlayer,
                message,
            });
    }

    private void ChatMessageEvent(object[] data)
    {
        _chatHistory.Enqueue(new Tuple<Player, string>((Player)data[0], (string)data[1]));
        OnNewChatMessage?.Invoke();
    }
}
