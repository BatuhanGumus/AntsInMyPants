using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;

    private bool activated;

    private void Awake()
    {
        GameData.GameFlow.OnGameStateChanged += GameStateChanged;
    }

    private void OnDestroy()
    {
        GameData.GameFlow.OnGameStateChanged -= GameStateChanged;
    }

    private void LateUpdate()
    {
        if (activated && GameData.RoomManager.LocalPlayer != null)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                GameData.RoomManager.LocalPlayer.transform.position + offset,
                Time.deltaTime * 8);
        }
    }

    private void GameStateChanged(GameState state)
    {
        if (state is InGameState)
        {
            activated = true;
        }
    }
}
