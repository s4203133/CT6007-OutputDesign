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
        if (numberOfSurroundingTiles == 0) {
            cellTile = TileMenu.Empty;
            newCell = GameObject.Instantiate(cellPrefab, position, Quaternion.identity);
            newCell.Init();
            CastleInterior.OnInteriorTileCreated?.Invoke(newCell.gameObject);
        }
        else {
            cellTile = combinations.GetCorrectTile(surroundingTiles);
            newCell = GameObject.Instantiate(edgePrefab, position, Quaternion.identity);
            newCell.Init();
            newCell.perimeterTile = true;
            if (!cellTile.StartCollapsed) {
                CastlePerimeter.OnPerimeterTileCreated?.Invoke(newCell.gameObject);
            }
        }

        if (cellTile.StartCollapsed) {
            newCell.Init();
            newCell.Collapse();
        }
        newCell.SetTile(cellTile);

        return newCell.gameObject;
    }

    public GameObject GetNullTile(Vector3 position) {
        Cell newCell = GameObject.Instantiate(nullCellPrefab, position, Quaternion.identity);
        newCell.Init();
        newCell.SetTile(TileMenu.Empty);
        newCell.Collapse();
        return newCell.gameObject;
    }
}
