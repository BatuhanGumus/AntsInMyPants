
using System;
using Photon.Realtime;

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
        else
        {
            return this;
        }
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
            if (GameData.OnlineManager.IsOwnerClient)
            {
                return new OwnerGameState();
            }
            else
            {
                return new RemoteGameState();
            }
            
        }
        else
        {
            return this;
        }
    }
}

public class OwnerGameState : GameState
{
    
}

public class RemoteGameState : GameState
{
    
}

public interface IGameFlow
{
   public GameState CurrentGameState { get; }
   public event Action<GameState> OnGameStateChanged;
}
