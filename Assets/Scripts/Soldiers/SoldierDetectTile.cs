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
        if (movingTowardsTile) {
            return;
        }

        Collider[] tiles = Physics.OverlapSphere(thisTransform.position, detectBreakableTileRange, tileLayers);
        if (tiles.Length > 0) {
            target = GetClosestTile(tiles).transform;
            movingTowardsTile = true;
        }
        else {
            target = null;
        }
    }

    private GameObject GetClosestTile(Collider[] tiles) {
        GameObject returnTile = tiles[0].gameObject;
        float mintDistance = detectBreakableTileRange * 2;
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
        movingTowardsTile = false;
        target = null;
        IsInRange = false;
    }
}
