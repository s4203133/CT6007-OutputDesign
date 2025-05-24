 using System.Collections.Generic;
using UnityEngine;

public abstract class WaveFunctionCollapseLayer : MonoBehaviour
{
    protected WaveFunctionCollapse waveFunctionCollapse;
    [SerializeField] private List<Tile> availableTiles;
    protected TileGrid grid;
    protected int iterations = 0;

    public List<Tile> GetPossibleTiles(SurroundingTiles surroundingTiles) {
        List<Tile> returnTiles = new List<Tile>();
        // If this is tile has a below tile, then it is a vertical cell so just get the possible above tiles and return
        if (surroundingTiles.below != null) {
            GetPossibleTiles(BelowTile(surroundingTiles).Above, ref returnTiles);
            return returnTiles;
        }
        // Get the tile that is north of this cell, and retrieve the possible tiles that can be placed south of it
        if (surroundingTiles.north != null) {
            GetPossibleTiles(NorthTile(surroundingTiles).South, ref returnTiles);
        }
        // Get the tile that is south of this cell, and retrieve the possible tiles that can be placed north of it
        if (surroundingTiles.south != null) {
            GetPossibleTiles(SouthTile(surroundingTiles).North, ref returnTiles);
        }
        // Get the tile that is east of this cell, and retrieve the possible tiles that can be placed west of it
        if (surroundingTiles.east != null) {
            GetPossibleTiles(EastTile(surroundingTiles).West, ref returnTiles);
        }
        // Get the tile that is west of this cell, and retrieve the possible tiles that can be placed east of it
        if (surroundingTiles.west != null) {
            GetPossibleTiles(WestTile(surroundingTiles).East, ref returnTiles);
        }

        CheckSurroundingTiles(surroundingTiles, ref returnTiles);

        return returnTiles;
    }

    private void GetPossibleTiles(List<Tile> targetTiles, ref List<Tile> listAddingTo) {
        // If the tile has no possible tiles in the target direction, return with nothing
        if (targetTiles.Count == 0) {
            return;
        }
        for (int i = 0; i < targetTiles.Count; i++) {
            // If all of the attack points have been spent, check if this tile is an attack tile, and don't include it if so
            if (waveFunctionCollapse.SpentAllAttackPoints) {
                if (targetTiles[i].AttackTile) { 
                    continue; 
                }
            }
            // If this type of tile isn't already in the list, and it is available on the current layer, add it to the possible tiles list
            if (!listAddingTo.Contains(targetTiles[i])) {
                if (availableTiles.Contains(targetTiles[i])) {
                    listAddingTo.Add(targetTiles[i]);
                }
            }
        }
    }

    private void CheckSurroundingTiles(SurroundingTiles surroundingTiles, ref List<Tile> listAddingTo) {
        // For each tile in the list, if one of the other tiles around can't have it placed next to them, remove it from the list
        for(int i = 0; i < listAddingTo.Count;i++) {
            Tile tile = listAddingTo[i];
            // If one of the surrounding tiles doesn't contain this tile in their possible tiles array, remove it from the list
            if (NorthTile(surroundingTiles).South.Count > 0 && !NorthTile(surroundingTiles).South.Contains(tile) || 
                SouthTile(surroundingTiles).North.Count > 0 && !SouthTile(surroundingTiles).North.Contains(tile) || 
                WestTile(surroundingTiles).East.Count > 0 && !WestTile(surroundingTiles).East.Contains(tile) || 
                EastTile(surroundingTiles).West.Count > 0 && !EastTile(surroundingTiles).West.Contains(tile)) 
                {
                listAddingTo.Remove(tile);
                i--;
            }
        }
    }

    public void AddIteration() {
        iterations++;
    }

    // Accessors to reduce and simplify syntax
    private Tile NorthTile(SurroundingTiles surroundingTiles) => surroundingTiles.north.GetTile();
    private Tile EastTile(SurroundingTiles surroundingTiles) => surroundingTiles.east.GetTile();
    private Tile SouthTile(SurroundingTiles surroundingTiles) => surroundingTiles.south.GetTile();
    private Tile WestTile(SurroundingTiles surroundingTiles) => surroundingTiles.west.GetTile();
    private Tile BelowTile(SurroundingTiles surroundingTiles) => surroundingTiles.below.GetTile();

    public abstract void Initialise(TileGrid grid, WaveFunctionCollapse wfc);
    public abstract bool Complete();
    public abstract bool TileCondition(Cell cell);
}
