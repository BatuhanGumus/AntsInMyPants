using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button sendButton;

    private void Awake()
    {
        sendButton.onClick.AddListener(SendMessage);
    }

    private void OnDestroy()
    {
        sendButton.onClick.RemoveListener(SendMessage);
    }

    private void SendMessage()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            GameData.ChatManager.SendChatMessage(inputField.text);
        }
    }
}
