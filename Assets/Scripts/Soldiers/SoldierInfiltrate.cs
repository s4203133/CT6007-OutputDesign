using UnityEngine;
using UnityEngine.AI;

[System.Serializable]   
public class SoldierInfiltrate
{
    private NavMeshAgent agent;
    private Transform target;

    public bool infiltrating { get; private set; }

    public void Initialise(NavMeshAgent agent, Transform castleTarget) {
        this.agent = agent; 
        target = castleTarget;
    }

    public void FindPathIntoCastle() {
        if (agent == null || target == null)
        {
            infiltrating = false;
            return;
        }
        NavMeshPath pathToCastle = new NavMeshPath();
        if (agent.CalculatePath(target.position, pathToCastle)) {
            agent.SetPath(pathToCastle);
            agent.stoppingDistance = 1f;
            infiltrating = true;
        }
        else {
            infiltrating = false;
        }
    }
}
