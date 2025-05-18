using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class SoldierSpawner : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;

    [Space(10)]
    [SerializeField] private int amount;
    [SerializeField] private GameObject soldierPrefab;

    public List<GameObject> soldiers { get; private set; }

    public static Action AllSoldiersKilled;

    private void OnEnable() {
        WaveFunctionCollapse.OnFinished += SpawnSoldiers;
        CastleTarget.OnSimulationEnded += DisableAllSoldiers;
        Soldier.OnKilled += RemoveSoldier;
    }

    private void OnDisable() {
        WaveFunctionCollapse.OnFinished -= SpawnSoldiers;
        CastleTarget.OnSimulationEnded -= DisableAllSoldiers;
        Soldier.OnKilled -= RemoveSoldier;
    }

    private void SpawnSoldiers() {
        soldiers = new List<GameObject>();
        navMeshSurface.BuildNavMesh();

        bool allSoldiersSpawned = false;
        int numOfSoldiers = 0;

        int x = -5;
        int y = -20;

        while (!allSoldiersSpawned) {
            soldiers.Add(Instantiate(soldierPrefab, new Vector3(x, 1, y), Quaternion.identity));

            x++;
            if(x >= 5) {
                x = -5;
                y--;
            }

            numOfSoldiers++;
            if(numOfSoldiers >= amount) {
                allSoldiersSpawned = true;
                break;
            }
        }

/*        for (int i = -5; i < 5; i++) {
            for (int j = 0; i < 10; j++) {
                soldiers.Add(Instantiate(soldierPrefab, new Vector3(i, 1, -20), Quaternion.identity));
            }
        }*/
    }

    private void DisableAllSoldiers() {
        int count = soldiers.Count;
        for(int i = 0; i < count; i++) {
            soldiers[i].SetActive(false);
        }
    }

    private void RemoveSoldier(GameObject soldier) {
        soldiers.Remove(soldier);
        if(soldiers.Count == 0) {
            AllSoldiersKilled?.Invoke();
        }
    }
}
