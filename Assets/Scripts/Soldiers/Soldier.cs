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
        // Choose a random point on the front of the castle to use as the initial move to target
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
        // If the soldier is heading towards the centre of the castle, they have finished their attack phase so return
        if (infiltrateCastle.infiltrating) {
            MoveToCentreOfCastle();
            return;
        }

        if (detectTile.HasTarget) {
            // If the target tile is in range, stop moving and start attacking the tile
            if (detectTile.InRange) {
                movement.Stop();
                attack.SetTarget(detectTile.TargetTile);
                attack.Begin();
                attack.Countdown();
            }
            // If the target tile is still not within range, continue moving towards it
            else {
                movement.Continue();
            }
        }
        else {
            detectTile.Clear();
            if (attack.IsAttacking) {
                // If the soldier has no target tile and has finished attacking, determine what to do next and return
                attack.Stop();
                DetermineNextAction();
                return;
            }
            // If the soldier has no target but hasn't begun attacking, find a target to move to
            FindAndMoveToTargetTile();
        }
    }

    private void DetermineNextAction() {
        // If the soldier has broken enough tiles, attempt to find a path to the centre of the castle
        if (attack.BrokenEnoughTiles) {
            infiltrateCastle.FindPathIntoCastle();
            movement.Continue();
        }
        else {
            // If the soldier hasn't broken enough tiles, find the next tile to move to and attack
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
        // Check if there are any tiles within range
        detectTile.CheckForTilesToBreak();
        if (detectTile.HasTarget) {
            // If a tile was find, move towards it
            movement.SetTarget(detectTile.TargetTile);
            movement.Continue();
            return;
        }
        else {
            // If no tile was found, keep moving towards the front area of the castle
            GameObject[] targets = GameObject.FindGameObjectsWithTag("CastleEntrance");
            movement.SetTarget(targets[UnityEngine.Random.Range(0, targets.Length)].transform);
            movement.Continue();
        }
    }

    private void MoveToCentreOfCastle() {
        movement.SetTarget(GameObject.FindGameObjectWithTag("Target").transform);
        movement.Continue();
    }
}
