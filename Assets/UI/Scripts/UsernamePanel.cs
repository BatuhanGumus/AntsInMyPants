using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UsernamePanel : MonoBehaviour
{
    public TMP_Text usernameText;
    public TMP_InputField inputField;
    public Button updateButton;

    private void Awake()
    {
        updateButton.onClick.AddListener(UpdateUsername);
        GameData.GameFlow.OnGameStateChanged += GameStateChanges;
    }

    private void GameStateChanges(GameState gameState)
    {
        if (gameState is MenuState)
        {
            UpdateUsernameText();
        }
    }

    private void UpdateUsernameText()
    {
        var username = GameData.SaveManager.CurrentSave.username;
        usernameText.text = (string.IsNullOrEmpty(username)) ? "____" : username;
    }

    private void UpdateUsername()
    {
        GameData.SaveManager.CurrentSave.username = inputField.text;
        GameData.SaveManager.MarkDirty();
        UpdateUsernameText();
    }

    private void OnDestroy()
    {
        updateButton.onClick.RemoveListener(UpdateUsername);
        GameData.GameFlow.OnGameStateChanged -= GameStateChanges;
    }
}
