using System;
using System.Collections.Generic;
using Custom.Extensions;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SpawnInfo
{
    public string creatureName;
    public Transform[] spawnPoints;
}

public class CreatureManager : MonoBehaviour, ICreatureManager
{
    public SpawnInfo[] spawnInfos = new SpawnInfo[0];
    private Dictionary<string, SpawnInfo> _spawnInfos = new Dictionary<string, SpawnInfo>();

    private void Awake()
    {
        if (GameData.CreatureManager == null)
        {
            GameData.CreatureManager = this;
        }
        else
        {
            Debug.LogError("[Creature Manager]: two instances of the game flow, destroying newest");
            Destroy(gameObject);
            
        }

        foreach (var spawnInfo in spawnInfos)
        {
            _spawnInfos.Add(spawnInfo.creatureName, spawnInfo);
        }
        
        GameData.GameFlow.OnGameStateChanged += GameStateChanges;
        GameData.NetworkEventManager.Events[NetworkEventType.CreatureCaught] += CreatureCaughtEventReceived;
    }

    private void OnDestroy()
    {
        GameData.NetworkEventManager.Events[NetworkEventType.CreatureCaught] -= CreatureCaughtEventReceived;
    }

    private Dictionary<int, Creature> _spawnedCreatures = new Dictionary<int, Creature>();

    private void GameStateChanges(GameState state)
    {
        if (state is InGameState && GameData.OnlineManager.IsRoomOwner)
        {
            RoomInit();
        }
    }

    private void RoomInit()
    {
        for (int i = 0; i < 20; i++)
        {
            SpawnCreature("Ant");
        }
    }

    private Creature SpawnCreature(string creatureName)
    {
        var spawnInfo = _spawnInfos[creatureName];
        
        var spawned = PhotonNetwork.Instantiate(
            creatureName,
            spawnInfo.spawnPoints.GetRandom().position,
            Quaternion.Euler(0, Random.Range(0, 360), 0),
            0,
            GetCreatureInstantiateData()).GetComponent<Creature>();

        return spawned;
    }

    private object[] GetCreatureInstantiateData()
    {
        return null;
    }

    public void CreatureSpawned(Creature creature)
    {
        _spawnedCreatures.Add(creature.GetComponent<PhotonView>().ViewID, creature);
    }

    public void CreatureDestroyed(Creature creature)
    {
        _spawnedCreatures.Remove(creature.GetComponent<PhotonView>().ViewID);
    }

    public void CreatureCaught(Creature creature)
    {
        GameData.SaveManager.CurrentSave.money += 100;
        GameData.SaveManager.MarkDirty();
        
        creature.gameObject.SetActive(false);

        if (GameData.OnlineManager.IsRoomOwner)
        {
            PhotonNetwork.Destroy(creature.gameObject);
            SpawnCreature("Ant");
        }
        else
            RemoteCreatureCaughtEventSend(creature);
    }

    private void RemoteCreatureCaughtEventSend(Creature creature)
    {
        GameData.NetworkEventManager.SendEvent(NetworkEventType.CreatureCaught, new object[]
        {
            creature.gameObject.GetComponent<PhotonView>().ViewID
        }, ReceiverGroup.MasterClient);
    }
    
    private void CreatureCaughtEventReceived(object[] data)
    {
        int id = (int) data[0];
        PhotonNetwork.Destroy(_spawnedCreatures[id].gameObject);
        SpawnCreature("Ant");
    }
}
