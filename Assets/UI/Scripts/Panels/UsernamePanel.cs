using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UsernamePanel : MonoBehaviour
{
    [Header("Input Elements")]
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button loginButton;

    [Header("Sub Panels")] 
    [SerializeField] private GameObject inputSubPanel;
    [SerializeField] private GameObject loadingSubPanel;

    private void Awake()
    {
        loginButton.onClick.AddListener(Login);
    }

    private void OnEnable()
    {
        inputSubPanel.SetActive(true);
        loadingSubPanel.SetActive(false);
        UpdateUsernameText();
    }

    private void UpdateUsernameText()
    {
        var username = GameData.SaveManager.CurrentSave.username;
        usernameText.text = (string.IsNullOrEmpty(username)) ? "____" : username;
    }

    private void Login()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            GameData.SaveManager.CurrentSave.username = inputField.text;
            GameData.SaveManager.MarkDirty();
            UpdateUsernameText();
        }

        if (!string.IsNullOrEmpty(GameData.SaveManager.CurrentSave.username))
        {
            GameData.OnlineManager.Login();
            
            inputSubPanel.SetActive(false);
            loadingSubPanel.SetActive(true);
        }
            
    }

    private void OnDestroy()
    {
        loginButton.onClick.RemoveListener(Login);
    }
}
