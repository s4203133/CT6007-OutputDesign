using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class SoldierMove
{
    [SerializeField] private NavMeshAgent agent;
    public GameObject currentTarget;
    public NavMeshAgent Agent => agent;

    public Action OnMovementStarted;
    public Action OnMovementStopped;

    public void Initialise() {
        // Add a small random offset to the stopping distance to add some variety to the soldiers
        agent.stoppingDistance += UnityEngine.Random.Range(-1f, 1f);
    }

    public void SetTarget(Transform newTarget) {
        agent.destination = newTarget.position;
        currentTarget = newTarget.gameObject;
    }

    public void Continue() {
        // Allow the nav mesh agent to move
        OnMovementStarted?.Invoke();
        agent.isStopped = false;
    }

    public void Stop() {
        // Pause the nav mesh agents movement 
        OnMovementStopped?.Invoke();
        agent.isStopped = true;
    }
}
