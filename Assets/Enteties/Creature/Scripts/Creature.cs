using System;
using UnityEngine;

public class Creature : MonoBehaviour
{
    private void Awake()
    {
        GameData.CreatureManager.CreatureSpawned(this);
    }

    private void OnDestroy()
    {
         GameData.CreatureManager.CreatureDestroyed(this);
    }
}
