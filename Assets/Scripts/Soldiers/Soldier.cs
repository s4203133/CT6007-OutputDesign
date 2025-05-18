using System;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    [SerializeField] private SoldierMove movement;
    [SerializeField] private SoldierDetectTile detectTile;
    [SerializeField] private SoldierAttack attack;
    [SerializeField] private SoldierInfiltrate infiltrateCastle;

    public NavMeshAgent Agent => movement.Agent;

    public static Action<GameObject> OnKilled;

    private void Start() {
        InitialiseComponents();
        GameObject[] targets = GameObject.FindGameObjectsWithTag("CastleEntrance");
        movement.SetTarget(targets[UnityEngine.Random.Range(0, targets.Length)].transform);
    }

    private void Update() {
        AttackTiles();
    }

    private void InitialiseComponents() {
        movement.Initialise();
        detectTile.Initialise(this);
        attack.Initialise(this);
        infiltrateCastle = new SoldierInfiltrate();
        infiltrateCastle.Initialise(movement.Agent, GameObject.FindGameObjectWithTag("Target").transform);
        GetComponentInChildren<SoldierAnimator>().Initialise(movement);
    }

    private void AttackTiles() {
        if (infiltrateCastle.infiltrating) {
            return;
        }

        if (detectTile.HasTarget) {
            if (detectTile.InRange) {
                movement.Stop();
                attack.SetTarget(detectTile.TargetTile);
                attack.Begin();
                attack.Countdown();
            }
        }
        else {
            detectTile.Clear();
            if (attack.IsAttacking) {
                attack.Stop();
                DetermineNextAction();
                return;
            }
            FindAndMoveToTargetTile();
        }
    }

    private void DetermineNextAction() {
        if (attack.BrokenEnoughTiles) {
            infiltrateCastle.FindPathIntoCastle();
            movement.Continue();
        }
        else {
            FindAndMoveToTargetTile();
        }
    }

    public CannonBall CreateAttackProjectile(CannonBall prefab) {
        return Instantiate(prefab, transform.position, transform.rotation);
    }

    private void OnDestroy() {
        OnKilled?.Invoke(gameObject);
    }

    private void FindAndMoveToTargetTile() {
        detectTile.CheckForTilesToBreak();
        if (detectTile.HasTarget) {
            movement.SetTarget(detectTile.TargetTile);
            movement.Continue();
            return;
        }
        else {
            MoveToCentreOfCastle();
        }
    }

    private void MoveToCentreOfCastle() {
        movement.SetTarget(GameObject.FindGameObjectWithTag("Target").transform);
        movement.Continue();
    }
}
