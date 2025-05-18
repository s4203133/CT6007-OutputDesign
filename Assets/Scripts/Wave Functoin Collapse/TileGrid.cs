using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileGrid {

    private List<Vector3> tilePlacements;
    private CastleData castleData;
    private LayerMask castleGround;

    [SerializeField] private TileGenerator tileGenerator;
    public TileGenerator generator => tileGenerator;

    //private List<GameObject> allTiles;
    public List<GameObject> Tiles => allTiles;

    private List<GameObject> allTiles;

    private CastlePerimeter castlePerimeter;
    private CastleInterior castleInterior;

    public void Initialise(CastleData data) {
        tilePlacements = new List<Vector3>();
        castleData = data;
        castleGround = 0;
        castleGround |= (1 << 6);

        allTiles = new List<GameObject>();
        tileGenerator.Init();

        castlePerimeter = new CastlePerimeter();
        castleInterior = new CastleInterior();
    }

    public void Generate() {
        int halfCastleWidth = (int)(castleData.width * 0.5f);
        int halfCastleLength = (int)(castleData.length * 0.5f);
        int index = 0;

        for(int i = -halfCastleWidth; i < halfCastleWidth; i++) {
            for(int j = -halfCastleLength; j < halfCastleLength; j++) {
                GameObject newCell = null;
                Vector3 tilePosition = new Vector3(i + 0.5f, 0, j + 0.5f);
                if (CellEmpty(tilePosition)) {
                    newCell = CreateCell(tilePosition, 0);
                }
                else {
                    newCell = CreateNullCell(tilePosition, 0);
                }

                newCell.GetComponent<Cell>().SetIndex(index);
                index++;
            }
        }
    }

    private GameObject CreateCell(Vector3 position, int height) {
        // 'surroundingTiles' stores either a 0 or 1 for each adjacent tile based on if it's empty or occupied
        int[] surroundingTiles = new int[8];
        int numOfSurroundingTiles = 0;
        int index = 0;
        for (int i = 1; i >= -1; i--) {
            for (int j = -1; j <= 1; j++) {
                if (i == 0 && j == 0) {
                    continue;
                }
                // check if the adjacent cells are empty (using i & j as the position offset)
                if (CellEmpty(position, j, i)) {
                    surroundingTiles[index] = 0;

                } else {
                    surroundingTiles[index] = 1;
                    numOfSurroundingTiles++;
                }
                index++;
            }
        }
        GameObject newTile = tileGenerator.GetTile(position, numOfSurroundingTiles, surroundingTiles);
        allTiles.Add(newTile.gameObject);
        return newTile;
    }

    private bool CellEmpty(Vector3 tilePosition) {
        if (Physics.Raycast(tilePosition + (Vector3.up * 5), Vector3.down, 10, castleGround)) {
            return false;
        }
        return true;
    }

    private bool CellEmpty(Vector3 tilePosition, int xOffset, int yOffset) {
        if (Physics.Raycast(tilePosition + new Vector3(xOffset, 5, yOffset), Vector3.down, 10, castleGround)) {
            return false;
        }
        return true;
    }

    public void ClearGrid() {
        int length = allTiles.Count;
        for(int i = 0; i < length; i++) {
            GameObject.Destroy(allTiles[i]);
        }
        tilePlacements.Clear();
        castlePerimeter = new CastlePerimeter();
        castleInterior = new CastleInterior();
    }

    private GameObject CreateNullCell(Vector3 position, int height) {
        GameObject newNullCell = tileGenerator.GetNullTile(position);
        allTiles.Add(newNullCell);
        return newNullCell;
    }

    public SurroundingTiles GetSurroundingTiles(int index) {
        SurroundingTiles returnTiles = new SurroundingTiles();
        if(index < 31 || index % 30 == 0 || (index + 1) % 30 == 0 || index > 870) {
            return returnTiles;
        }
        returnTiles.north = allTiles[index + 1].GetComponent<Cell>();
        returnTiles.east = allTiles[index + castleData.width].GetComponent<Cell>();
        returnTiles.south = allTiles[index - 1].GetComponent<Cell>();
        returnTiles.west = allTiles[index - castleData.width].GetComponent<Cell>();
        return returnTiles;
    }

    public SurroundingTiles GetSurroundingTiles(Cell cell) {
        return cell.surroundingTiles;
    }

    public bool AllPerimeterCellsCollapsed() {
        return castlePerimeter.AllTilesCollapsed();
    }

    public bool AllInteriorCellsCollapsed() {
        return castleInterior.AllTilesCollapsed();
    }

    public void SetGrid(List<GameObject> newGridTiles) {
        allTiles = newGridTiles;
    }
}
