using System;
using UnityEngine;

[Serializable]
public class SoldierDetectTile
{
    private Transform thisTransform;
    private Transform target;

    [SerializeField] private float detectBreakableTileRange;
    [SerializeField] private LayerMask tileLayers;
    private float stoppingRange;

    private bool movingTowardsTile;

    public Transform TargetTile => target;
    public bool HasTarget => target != null;

    public bool InRange => Vector3.Distance(thisTransform.position, target.position) <= (stoppingRange + 1f);

    public bool IsInRange;
    public void Initialise(Soldier soldier) {
        thisTransform = soldier.transform;
        stoppingRange = soldier.Agent.stoppingDistance;
        movingTowardsTile = false;
    }

    public void CheckForTilesToBreak() {
        // If the soldier already has a target tile and is moving towards it, return
        if (movingTowardsTile) {
            return;
        }

        // Find all tiles within range
        Collider[] tiles = Physics.OverlapSphere(thisTransform.position, detectBreakableTileRange, tileLayers);
        // If 1 tile was found, set it to be the target
        if(tiles.Length == 1) {
            target = tiles[0].transform;
            movingTowardsTile = true;
        }
        // If more than 1 tile was found, calculate the closest one and set that to be the target
        if (tiles.Length > 0) {
            target = GetClosestTile(tiles).transform;
            movingTowardsTile = true;
        }
        // If no tiles were found, ensure the target is null
        else {
            target = null;
        }
    }

    private GameObject GetClosestTile(Collider[] tiles) {
        GameObject returnTile = tiles[0].gameObject;
        float mintDistance = detectBreakableTileRange * 2;
        // For each tile, calculate the one with the least distance to the soldier
        for(int i = 0; i < tiles.Length; i++) {
            float distance = Vector3.Distance(thisTransform.position, tiles[i].transform.position);
            if (distance < mintDistance) {
                returnTile = tiles[i].gameObject;
                mintDistance = distance; 
            }
        }
        return returnTile;
    }

    public void Clear() {
        // Reset variables
        movingTowardsTile = false;
        target = null;
        IsInRange = false;
    }
}
