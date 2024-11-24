
using System;

public abstract class GameState
{
    public virtual void Start(){}
    public virtual void Update(){}

    public virtual GameState End()
    {
        return null;
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
}

public class MenuState : GameState
{
    
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
