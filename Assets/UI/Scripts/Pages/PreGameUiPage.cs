using System;
using UnityEngine;

public class PreGameUiPage : MonoBehaviour
{
    [SerializeField] private GameObject usernamePanel;
    [SerializeField] private GameObject lobbyPanel;

    private void Awake()
    {
        GameData.GameFlow.OnGameStateChanged += GameStateChanged;
    }

    private void GameStateChanged(GameState gameState)
    {
        if (gameState is LoginGameState)
        {
            usernamePanel.SetActive(true);
            lobbyPanel.SetActive(false);
        }
        else if (gameState is LobbyMenuGameState)
        {
            usernamePanel.SetActive(false);
            lobbyPanel.SetActive(true);
        }
        else
        {
            usernamePanel.SetActive(false);
            lobbyPanel.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        GameData.GameFlow.OnGameStateChanged -= GameStateChanged;
    }
}
