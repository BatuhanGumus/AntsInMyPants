
using System;
using System.Collections.Generic;
using Photon.Realtime;

public interface IChatManager
{
    Queue<Tuple<Player, string>> ChatHistory { get; }
    Action<Player,string> OnNewChatMessage { get; set; }
    Action<bool> OnChatMode { get; set; }
    void SendChatMessage(string message);
    bool InChatMode { get; }
    void ChatMode(bool state);
}
