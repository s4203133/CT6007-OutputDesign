using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VerticalGrid
{
    public List<GameObject> cells;
    private List<GameObject> baseCells;
    TileGenerator tileGenerator;

    public VerticalGrid(List<GameObject> cells, TileGenerator generator) {
        baseCells = cells;
        this.cells = new List<GameObject>();
        tileGenerator = generator;
    }

    public void Generate() {
        int count = baseCells.Count;
        for(int i = 0; i < count; i++) {
            // For each tile, if it can have a tile placed above it, then create a new cell at the right position and add it to the list
            if (baseCells[i].GetComponent<Cell>().GetTile().Above.Count > 0) {
                GameObject newCell = tileGenerator.GetTile(baseCells[i].transform.position + Vector3.up, 0, new int[0]);
                newCell.GetComponent<Cell>().surroundingTiles.below = baseCells[i].GetComponent<Cell>();
                cells.Add(newCell);
            }
        }
    }
}
