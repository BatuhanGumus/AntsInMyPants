using System;
using System.Collections.Generic;
using Custom.Extensions;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreatureManager : MonoBehaviour, ICreatureManager
{
    public Transform[] creatureSpawnPoints;

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
        
        GameData.GameFlow.OnGameStateChanged += GameStateChanges;
    }

    private List<Creature> _spawnedCreatures = new List<Creature>();

    private void GameStateChanges(GameState state)
    {
        if (state is InGameState && GameData.OnlineManager.IsRoomOwner)
        {
            RoomInit();
        }
    }

    private void RoomInit()
    {
        for (int i = 0; i < 40; i++)
        {
            SpawnCreature();
        }
    }

    private Creature SpawnCreature()
    {
        var spawned = PhotonNetwork.Instantiate(
            "Creature",
            creatureSpawnPoints.GetRandom().position,
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
        _spawnedCreatures.Add(creature);
    }

    public void CreatureDestroyed(Creature creature)
    {
        _spawnedCreatures.Remove(creature);
    }

    public void CreatureCaught(Creature creature)
    {
        GameData.SaveManager.CurrentSave.money += 100;
        GameData.SaveManager.MarkDirty();
        
        PhotonNetwork.Destroy(creature.gameObject);
    }
}
