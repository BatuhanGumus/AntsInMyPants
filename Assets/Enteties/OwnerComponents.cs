using System;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class OwnerComponents : MonoBehaviour
{
    private PhotonView _networkView;

    [SerializeField] private MonoBehaviour[] ownerComponents;
    [SerializeField] private MonoBehaviour[] remoteComponents;

    private void Awake()
    {
        _networkView = gameObject.GetComponent<PhotonView>();
    }

    private void Start()
    {
        var owner = _networkView.IsMine;
        foreach (var component in ownerComponents)
        {
            component.enabled = owner;
        }

        foreach (var component in remoteComponents)
        {
            component.enabled = !owner;
        }
    }
}
