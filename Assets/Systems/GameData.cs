using UnityEngine;

public static class GameData
{
    public static IGameFlow GameFlow;
    public static ISaveManager SaveManager;
    public static IOnlineManager OnlineManager;
    public static IRoomManager RoomManager;
    public static INetworkEventManager NetworkEventManager;
    public static IChatManager ChatManager;
    public static ICreatureManager CreatureManager;

    public static void Init()
    {
        SaveManager = new SaveManager();
        NetworkEventManager = new NetworkEventManager();
        ChatManager = new ChatManager();
    }
}
