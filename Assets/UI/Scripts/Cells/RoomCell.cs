using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomCell : MonoBehaviour
{
    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private TMP_Text roomPlayersText;
    [SerializeField] private Button joinRoomButton;

    private string _roomName;
    private Action _onJoinedClick;

    public void Start()
    {
        joinRoomButton.onClick.AddListener(() =>
        {
            GameData.RoomManager.JoinRoom(_roomName);
            _onJoinedClick?.Invoke();
        });
    }

    public void SetData(string roomName, byte currentPlayers, byte maxPlayers, Action onJoinClicked)
    {
        _roomName = roomName;
        _onJoinedClick += onJoinClicked;

        roomNameText.text = _roomName;
        roomPlayersText.text = currentPlayers + " / " + maxPlayers;
    }
}