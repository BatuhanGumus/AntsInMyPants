using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class GamePlayer : MonoBehaviour, IPunInstantiateMagicCallback
{
    [SerializeField] private TMP_Text usernameText;

    [HideInInspector] public Player OwnerPlayer;
    private string _username;
    
    private Camera _cam;
    
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        OwnerPlayer = info.photonView.Owner;
        _username = info.photonView.InstantiationData[0] as string;

        usernameText.text = _username;
        
        GameData.RoomManager.RegisterSpawnedPlayer(OwnerPlayer, this);
    }

    private void Start()
    {
        _cam = Camera.main;
    }

    private void LateUpdate()
    {
        usernameText.transform.LookAt(usernameText.transform.position + _cam.transform.forward);
    }
}
