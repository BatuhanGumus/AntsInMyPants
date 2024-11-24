using System;
using UnityEngine;

public class GameFlow : MonoBehaviour, IGameFlow
{
    public GameState CurrentGameState => _currentGameState;
    public event Action<GameState> OnGameStateChanged;

    private GameState _currentGameState;
    
    private void Awake()
    {
        if (GameData.GameFlow == null)
        {
            GameData.GameFlow = this;
            GameData.Init();
        }
        else
        {
            Debug.LogError("[Game Flow]: two instances of the game flow, destroying newest");
            Destroy(gameObject);
            return;
        }
        
        _currentGameState = new PreGameState();
        _currentGameState.Start();
        OnGameStateChanged?.Invoke(_currentGameState);
    }

    private void Start()
    {
        _currentGameState = new MenuState();
        OnGameStateChanged?.Invoke(_currentGameState);
    }

    private void Update()
    {
        if (_currentGameState != null)
        {
            //_currentGameState.Update();
        }
    }
}
