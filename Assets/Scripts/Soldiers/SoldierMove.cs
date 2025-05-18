using System;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class SoldierMove
{
    [SerializeField] private NavMeshAgent agent;
    public NavMeshAgent Agent => agent;
    public bool stopped;

    public Action OnMovementStarted;
    public Action OnMovementStopped;

    public void Initialise() {
        agent.stoppingDistance += UnityEngine.Random.Range(-1f, 1f);
    }

    public void SetTarget(Transform newTarget) {
        agent.destination = newTarget.position;
    }

    public void Continue() {
        OnMovementStarted?.Invoke();
        agent.isStopped = false;
        stopped = false;
    }

    public void Stop() {
        OnMovementStopped?.Invoke();
        agent.isStopped = true;
        stopped = true;
    }
}
