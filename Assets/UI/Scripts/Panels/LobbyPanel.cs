using System;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Photon.Pun.Demo.Asteroids
{
    public class LobbyPanel : MonoBehaviour
    {
        [Header("Sub Panels")] 
        [SerializeField] private GameObject contentSubPanel;
        [SerializeField] private GameObject loadingSubPanel;
        
        [Header("Create Room")]
        [SerializeField] private TMP_InputField roomNameInputField;
        [SerializeField] private Button createRoomButton;

        [Header("Room List")]
        [SerializeField] private GameObject roomListContent;
        [SerializeField] private GameObject roomListEntryPrefab;
        
        
        private Dictionary<string, GameObject> _roomListEntries;

        public void Awake()
        {
            _roomListEntries = new Dictionary<string, GameObject>();
            GameData.RoomManager.OnCachedRoomListUpdated += UpdateRoomListView;
            createRoomButton.onClick.AddListener(OnCreateRoomButtonClicked);
        }

        private void OnEnable()
        {
            ShowLoading(false);
        }

        private void OnDestroy()
        {
            GameData.RoomManager.OnCachedRoomListUpdated -= UpdateRoomListView;
            createRoomButton.onClick.RemoveListener(OnCreateRoomButtonClicked);
        }
        
        public void OnCreateRoomButtonClicked()
        {
            string roomName = roomNameInputField.text;
            GameData.RoomManager.CreateRoom(roomName);
            ShowLoading(true);
        }

        private void ShowLoading(bool isLoading)
        {
            contentSubPanel.SetActive(!isLoading);
            loadingSubPanel.SetActive(isLoading);
        }

        private void ClearRoomListView()
        {
            foreach (GameObject entry in _roomListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            _roomListEntries.Clear();
        }

        private void UpdateRoomListView()
        {
            ClearRoomListView();
            
            foreach (RoomInfo info in GameData.RoomManager.CachedRoomList.Values)
            {
                GameObject entry = Instantiate(roomListEntryPrefab);
                entry.transform.SetParent(roomListContent.transform);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<RoomCell>().SetData(
                    info.Name, 
                    (byte)info.PlayerCount, 
                    (byte)info.MaxPlayers, 
                    () => ShowLoading(true));

                _roomListEntries.Add(info.Name, entry);
            }
        }
    }
}