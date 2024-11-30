using System;
using Custom.Extensions;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class LocalCreatureController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    private void Start()
    {
        SetNewDestination();
    }

    private void Update()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            SetNewDestination();
        }
    }

    private void SetNewDestination()
    {
        agent.destination = transform.position + Quaternion.Euler(0, Random.Range(0f, 360f), 0) * transform.forward * Random.Range(6, 12f);
    }
}
