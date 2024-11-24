using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionStatusPanel : MonoBehaviour
{
    private readonly string connectionStatusMessage = "    Connection Status: ";
    
    [SerializeField] private TMP_Text ConnectionStatusText;

    public void Update()
    {
        ConnectionStatusText.text = connectionStatusMessage + GameData.OnlineManager.OnlineState;
    }
}