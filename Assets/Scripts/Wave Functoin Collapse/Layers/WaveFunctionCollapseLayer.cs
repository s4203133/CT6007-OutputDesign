using System.Collections.Generic;
using UnityEngine;

public abstract class WaveFunctionCollapseLayer : MonoBehaviour
{
    [SerializeField] private List<Tile> availableTiles;
    protected TileGrid grid;

    public List<Tile> GetPossibleTiles(SurroundingTiles surroundingTiles) {
        List<Tile> returnTiles = new List<Tile>();
        if (surroundingTiles.below != null) {
            GetPossibleTiles(BelowTile(surroundingTiles).Above, ref returnTiles);
            return returnTiles;
        }
        if (surroundingTiles.north != null) {
            GetPossibleTiles(NorthTile(surroundingTiles).South, ref returnTiles);
        }
        if (surroundingTiles.south != null) {
            GetPossibleTiles(SouthTile(surroundingTiles).North, ref returnTiles);
        }
        if (surroundingTiles.east != null) {
            GetPossibleTiles(EastTile(surroundingTiles).West, ref returnTiles);
        }
        if (surroundingTiles.west != null) {
            GetPossibleTiles(WestTile(surroundingTiles).East, ref returnTiles);
        }
        CheckSurroundingTiles(surroundingTiles, ref returnTiles);
        return returnTiles;
    }

    private void GetPossibleTiles(List<Tile> targetTiles, ref List<Tile> listAddingTo) {
        if (targetTiles.Count == 0) {
            return;
        }
        for (int i = 0; i < targetTiles.Count; i++) {
            if (!listAddingTo.Contains(targetTiles[i])) {
                if (availableTiles.Contains(targetTiles[i])) {
                    listAddingTo.Add(targetTiles[i]);
                }
            }
        }
    }

    private void CheckSurroundingTiles(SurroundingTiles surroundingTiles, ref List<Tile> listAddingTo) {
        for(int i = 0; i < listAddingTo.Count;i++) {
            Tile tile = listAddingTo[i];
            if (NorthTile(surroundingTiles).South.Count > 0 && !NorthTile(surroundingTiles).South.Contains(tile) || 
                SouthTile(surroundingTiles).North.Count > 0 && !SouthTile(surroundingTiles).North.Contains(tile) || 
                WestTile(surroundingTiles).East.Count > 0 && !WestTile(surroundingTiles).East.Contains(tile) || 
                EastTile(surroundingTiles).West.Count > 0 && !EastTile(surroundingTiles).West.Contains(tile) 
                /*BelowTile(surroundingTiles).Above.Count > 0 && !BelowTile(surroundingTiles).Above.Contains(tile) */) {
                listAddingTo.Remove(tile);
                i--;
            }
        }
    }

    private Tile NorthTile(SurroundingTiles surroundingTiles) => surroundingTiles.north.GetTile();
    private Tile EastTile(SurroundingTiles surroundingTiles) => surroundingTiles.east.GetTile();
    private Tile SouthTile(SurroundingTiles surroundingTiles) => surroundingTiles.south.GetTile();
    private Tile WestTile(SurroundingTiles surroundingTiles) => surroundingTiles.west.GetTile();
    private Tile BelowTile(SurroundingTiles surroundingTiles) => surroundingTiles.below.GetTile();

    public abstract void Initialise(TileGrid grid, WaveFunctionCollapse wfc);
    public abstract bool Complete();
    public abstract bool TileCondition(Cell cell);
}
