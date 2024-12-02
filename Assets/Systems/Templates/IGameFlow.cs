
using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public abstract class GameState
{
    public virtual void Start(){}

    public virtual GameState Update()
    {
        return this;
    }
}

public class PreGameState : GameState
{
    public override void Start()
    {
        GameData.SaveManager.GetSavedData();
    }

    public override GameState Update()
    {
        return new LoginGameState();
    }
}

public class LoginGameState : GameState
{
    public override GameState Update()
    {
        if (GameData.OnlineManager.OnlineState == ClientState.JoinedLobby)
        {
            return new LobbyMenuGameState();
        }
        
        return this;
    }
}

public class LobbyMenuGameState : GameState
{
    public override GameState Update()
    {
        if (GameData.OnlineManager.OnlineState == ClientState.Joined)
        {
            return new InGameState();
        }
        
        return this;
    }
}

public class InGameState : GameState
{
    public override GameState Update()
    {
        if (GameData.OnlineManager.OnlineState == ClientState.JoinedLobby)
        {
            return new LobbyMenuGameState();
        }
        
        return this;
    }
}


public interface IGameFlow
{
   public GameState CurrentGameState { get; }
   public event Action<GameState> OnGameStateChanged;
}
