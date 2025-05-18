using System;
using UnityEngine;

public class CastleTarget : MonoBehaviour
{
    [SerializeField] private GameObject successUI;
    [SerializeField] private GameObject failUI;

    public static Action OnSimulationEnded;
    public static Action OnSuccess;
    public static Action OnFail;

    private void OnEnable() {
        SoldierSpawner.AllSoldiersKilled += SimulationSucceeded;
    }

    private void OnDisable() {
        SoldierSpawner.AllSoldiersKilled -= SimulationSucceeded;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 8) {
            SimulationFailed();
        }
    }

    private void SimulationSucceeded() {
        successUI.SetActive(true);
        OnSimulationEnded?.Invoke();
        OnSuccess?.Invoke();
    }

    private void SimulationFailed() {
        failUI.SetActive(true);
        OnSimulationEnded?.Invoke();
        OnFail?.Invoke();
    }
}
