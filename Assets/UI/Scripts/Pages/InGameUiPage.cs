using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameUiPage : MonoBehaviour
{
    [SerializeField] private GameObject chatPanel;
    [SerializeField] private GameObject optionsPanel;
    
    private InputAction _optionsMenu;

    private bool _isInGame = false;
    
    private void Awake()
    {
        _optionsMenu = InputSystem.actions.FindAction("OptionsMenu");
        
        GameData.GameFlow.OnGameStateChanged += GameStateChanged;
    }

    private void Update()
    {
        if (_isInGame && _optionsMenu.WasPressedThisFrame() && !GameData.ChatManager.InChatMode)
        {
            optionsPanel.SetActive(!optionsPanel.activeSelf);
        }
    }

    private void GameStateChanged(GameState gameState)
    {
        if (gameState is InGameState)
        {
            _isInGame = true;
            chatPanel.SetActive(true);
        }
        else
        {
            _isInGame = false;
            chatPanel.SetActive(false);
            optionsPanel.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        GameData.GameFlow.OnGameStateChanged -= GameStateChanged;
    }
}
