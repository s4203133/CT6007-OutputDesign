using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileGrid {

    private List<Vector3> tilePlacements;
    private CastleData castleData;
    private LayerMask castleGround;

    [SerializeField] private TileGenerator tileGenerator;
    public TileGenerator generator => tileGenerator;

    private List<GameObject> allTiles;
    public List<GameObject> Tiles => allTiles;

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
        // Using negattive to positive castle width and height so that the centre of the castle is placed at the centre of the world
        // (Rather than the bottom left corner of the castle being placed at the centre)
        int halfCastleWidth = (int)(castleData.width * 0.5f);
        int halfCastleLength = (int)(castleData.length * 0.5f);
        int index = 0;

        for(int i = -halfCastleWidth; i < halfCastleWidth; i++) {
            for(int j = -halfCastleLength; j < halfCastleLength; j++) {
                // Create a new tile at the correct position
                GameObject newCell = null;
                Vector3 tilePosition = new Vector3(i + 0.5f, 0, j + 0.5f);
                // If there is no floor from the BSP algorithm at this location, then this cell part of the castle
                if (CellEmpty(tilePosition)) {
                    newCell = CreateCell(tilePosition, 0);
                }
                // If there is floor from the BSP algorithm at this location, then this cell isn't part of the castle 
                else {
                    newCell = CreateNullCell(tilePosition);
                }
                // Provide the cell with an index id
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
        // Check all 8 adjecent cells
        for (int i = 1; i >= -1; i--) {
            for (int j = -1; j <= 1; j++) {
                // If both i and j are 0 then this is the centre cell, so don't check this one
                if (i == 0 && j == 0) {
                    continue;
                }
                // Check if the adjacent cells are empty (using i & j as the position offset)
                if (CellEmpty(position, j, i)) {
                    surroundingTiles[index] = 0;

                } else {
                    surroundingTiles[index] = 1;
                    numOfSurroundingTiles++;
                }
                index++;
            }
        }
        // Determine where a tile is on the grid and get the correct type of tile based on what the adjacent cells are
        GameObject newTile = tileGenerator.GetTile(position, numOfSurroundingTiles, surroundingTiles);
        allTiles.Add(newTile.gameObject);
        return newTile;
    }

    private bool CellEmpty(Vector3 tilePosition) {
        // If there is ground from the BSP algorithm at this location, then this is not an empty space to place a castle cell
        if (Physics.Raycast(tilePosition + (Vector3.up * 5), Vector3.down, 10, castleGround)) {
            return false;
        }
        return true;
    }

    private bool CellEmpty(Vector3 tilePosition, int xOffset, int yOffset) {
        // If there is ground from the BSP algorithm at this location, then this is not an empty space to place a castle cell
        if (Physics.Raycast(tilePosition + new Vector3(xOffset, 5, yOffset), Vector3.down, 10, castleGround)) {
            return false;
        }
        return true;
    }

    private GameObject CreateNullCell(Vector3 position) {
        // Create a blank cell (to be used for any cell that isn't part of the castle)
        GameObject newNullCell = tileGenerator.GetNullTile(position);
        allTiles.Add(newNullCell);
        return newNullCell;
    }

    public SurroundingTiles GetSurroundingTiles(int index) {
        SurroundingTiles returnTiles = new SurroundingTiles();
        // If the cell is on the edges of the whole map, don't check for surrounding tiles
        if(index < 31 || index % 30 == 0 || (index + 1) % 30 == 0 || index > 870) {
            return returnTiles;
        }
        // Get each cell that is directly next to this one
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
