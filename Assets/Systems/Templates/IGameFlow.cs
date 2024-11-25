
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

    public virtual bool DoneCondition()
    {
        return false;
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
    public override bool DoneCondition()
    {
        return GameData.OnlineManager.OnlineState == ClientState.JoinedLobby;
    }

    public override GameState Update()
    {
        if (DoneCondition())
        {
            return new LobbyMenuGameState();
        }
        
        return this;
    }
}

public class LobbyMenuGameState : GameState
{
    public override bool DoneCondition()
    {
        return GameData.OnlineManager.OnlineState == ClientState.Joined;
    }

    public override GameState Update()
    {
        if (DoneCondition())
        {
            return new InGameState();
        }
        
        return this;
    }
}

public class InGameState : GameState
{
    public override void Start()
    {
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);  
    }
}


public interface IGameFlow
{
   public GameState CurrentGameState { get; }
   public event Action<GameState> OnGameStateChanged;
}
