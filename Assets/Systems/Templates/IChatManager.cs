
using System;
using System.Collections.Generic;
using Photon.Realtime;

public interface IChatManager
{
    Queue<Tuple<Player, string>> ChatHistory { get; }
    Action OnNewChatMessage { get; }
    void SendChatMessage(string message);
}
