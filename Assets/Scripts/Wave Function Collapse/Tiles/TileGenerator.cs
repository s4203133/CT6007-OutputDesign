using UnityEngine;

[System.Serializable]
public class TileGenerator {

    [SerializeField] Cell cellPrefab;
    [SerializeField] Cell edgePrefab;
    [SerializeField] Cell nullCellPrefab;
    CellCombinations combinations;

    public void Init() {
        combinations = new CellCombinations(0);
    }

    public GameObject GetTile(Vector3 position, int numberOfSurroundingTiles, int[] surroundingTiles) {
        Cell newCell;
        Tile cellTile;
        // If there are no surrounding cells, then create a blank cell and mark it as an interior cell
        if (numberOfSurroundingTiles == 0) {
            cellTile = TileMenu.Empty;
            newCell = GameObject.Instantiate(cellPrefab, position, Quaternion.identity);
            newCell.Init();
            CastleInterior.OnInteriorTileCreated?.Invoke(newCell.gameObject);
        }
        else {
            // If there is at least 1 surrounding cell, then determine which type of tile to create
            cellTile = combinations.GetCorrectTile(surroundingTiles);
            newCell = GameObject.Instantiate(edgePrefab, position, Quaternion.identity);
            newCell.Init();
            // Mark this cell as a perimeter cell
            newCell.perimeterTile = true;
            if (!cellTile.StartCollapsed) {
                CastlePerimeter.OnPerimeterTileCreated?.Invoke(newCell.gameObject);
            }
        }

        // Collapse the cell if it should start that way (used for corners, as this bakes them into the map for the walls to be created off)
        if (cellTile.StartCollapsed) {
            newCell.Init();
            newCell.Collapse();
        }
        newCell.SetTile(cellTile);

        return newCell.gameObject;
    }

    public GameObject GetNullTile(Vector3 position) {
        // Get a null tile to use for cells that aren't part of the castle (cells outside in the surrounding area)
        Cell newCell = GameObject.Instantiate(nullCellPrefab, position, Quaternion.identity);
        newCell.Init();
        newCell.SetTile(TileMenu.Empty);
        newCell.Collapse();
        return newCell.gameObject;
    }
}
