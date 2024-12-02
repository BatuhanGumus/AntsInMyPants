using System;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChatPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button sendButton;
    [SerializeField] private TMP_Text messages;
    
    private InputAction _enterChatMode;
    private InputAction _sendChat;
    private InputAction _optionsMenu;
    
    private void Awake()
    {
        _enterChatMode = InputSystem.actions.FindAction("OpenChat");
        _sendChat = InputSystem.actions.FindAction("SendMessage");
        _optionsMenu = InputSystem.actions.FindAction("OptionsMenu");
        
        sendButton.onClick.AddListener(SendMessage);
        GameData.ChatManager.OnNewChatMessage += ReceiveChatMessage;
        inputField.onSelect.AddListener( str => GameData.ChatManager.ChatMode(true));
        inputField.onDeselect.AddListener( str => GameData.ChatManager.ChatMode(false));
        
        messages.text = "";
    }

    private void Update()
    {
        if (GameData.ChatManager.InChatMode)
        {
            if (_sendChat.WasPressedThisFrame())
            {
                SendMessage();
            }

            if (_optionsMenu.WasPressedThisFrame())
            {
                inputField.OnDeselect(new BaseEventData(EventSystem.current));
            }
        }
        else
        {
            if (_enterChatMode.WasPressedThisFrame())
            {
                inputField.OnSelect(new BaseEventData(EventSystem.current));
            }
        }
    }

    private void OnDestroy()
    {
        sendButton.onClick.RemoveListener(SendMessage);
        GameData.ChatManager.OnNewChatMessage -= ReceiveChatMessage;
    }

    private void SendMessage()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            GameData.ChatManager.SendChatMessage(inputField.text);
            inputField.text = "";
        }
        
        inputField.OnDeselect(new BaseEventData(EventSystem.current));
    }

    private void ReceiveChatMessage(Player from, string message)
    {
        messages.text = messages.text + $"\n{from.NickName}: {message}";
    }
}
