using UnityEngine;

public class InGameUiPage : MonoBehaviour
{
    [SerializeField] private GameObject chatPanel;
    
    private void Awake()
    {
        GameData.GameFlow.OnGameStateChanged += GameStateChanged;
    }

    private void GameStateChanged(GameState gameState)
    {
        if (gameState is InGameState)
        {
            chatPanel.SetActive(true);
        }
        else
        {
            chatPanel.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        GameData.GameFlow.OnGameStateChanged -= GameStateChanged;
    }
}
