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
        NavMeshPath pathToCastle = new NavMeshPath();
        if (agent.CalculatePath(target.position, pathToCastle)) {
            // If a path to the castle target was found, move towards it and register that the soldier is now in the infiltrating phase
            agent.SetPath(pathToCastle);
            agent.stoppingDistance = 1f;
            infiltrating = true;
        }
        else {
            // If no path was found, register that the soldier isn't in the infiltrating phase yet
            infiltrating = false;
        }
    }
}
