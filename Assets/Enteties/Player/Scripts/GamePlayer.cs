using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class GamePlayer : MonoBehaviour, IPunInstantiateMagicCallback
{
    private class BubbleData
    {
        public float time = 0;
        public GameObject bubbeObject;
    }

    private const int MaxBubbleCount = 3;
    private const int BubbleStayTime = 5;
    
    [SerializeField] private Transform canvas;
    [SerializeField] private Transform chatBubbleParent;
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private GameObject chatBubbleCellPrefab;

    public Player OwnerPlayer;
    private string _username;
    
    private Camera _cam;

    private Queue<BubbleData> _bubbleQueue = new Queue<BubbleData>();

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        OwnerPlayer = info.photonView.Owner;
        _username = info.photonView.InstantiationData[0] as string;

        usernameText.text = _username;
        
        GameData.RoomManager.RegisterSpawnedPlayer(OwnerPlayer, this);
    }

    private void Awake()
    {
        GameData.ChatManager.OnNewChatMessage += ReceiveChatMessage;
    }

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        foreach (var bubbleData in _bubbleQueue)
        {
            bubbleData.time += Time.deltaTime;
        }

        if (_bubbleQueue.TryPeek(out var result) && result.time > BubbleStayTime)
        {
            Destroy(_bubbleQueue.Dequeue().bubbeObject);
        }
    }

    private void LateUpdate()
    {
        canvas.transform.LookAt(canvas.transform.position + _cam.transform.forward);
    }
    
    private void ReceiveChatMessage(Player from, string message)
    {
        if (from.ActorNumber == OwnerPlayer.ActorNumber)
        {
            var spawnedBubble = Instantiate(chatBubbleCellPrefab, parent: chatBubbleParent);
            spawnedBubble.GetComponentInChildren<TMP_Text>().text = message;
            var bubbleData = new BubbleData();
            bubbleData.bubbeObject = spawnedBubble;
            _bubbleQueue.Enqueue(bubbleData);

            if (_bubbleQueue.Count > MaxBubbleCount)
            {
                Destroy(_bubbleQueue.Dequeue().bubbeObject);
            }
        }
    }
}
