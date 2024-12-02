using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
    public Button leaveLobby;
    public Button quitGame;

    private void Awake()
    {
        leaveLobby.onClick.AddListener(LeaveRoom);
        quitGame.onClick.AddListener(QuitGame);
    }

    private void OnDestroy()
    {
        leaveLobby.onClick.RemoveListener(LeaveRoom);
        quitGame.onClick.RemoveListener(QuitGame);
    }

    private void LeaveRoom()
    {
        GameData.RoomManager.LeaveRoom();
    }

    private void QuitGame()
    {
        GameData.RoomManager.LeaveRoom();
        Application.Quit();
    }
}
