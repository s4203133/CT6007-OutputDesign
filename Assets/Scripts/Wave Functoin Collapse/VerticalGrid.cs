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
            if (baseCells[i].GetComponent<Cell>().GetTile().Above.Count > 0) {
                GameObject newCell = tileGenerator.GetTile(baseCells[i].transform.position + Vector3.up, 0, new int[0]);
                newCell.GetComponent<Cell>().surroundingTiles.below = baseCells[i].GetComponent<Cell>();
                cells.Add(newCell);
            }
        }
    }

    private void CreateSurroundingTiles(GameObject cell) {
        for(int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (i == 0 && j == 0) {
                    continue;
                }
                GameObject newCell = tileGenerator.GetTile(cell.transform.position + Vector3.forward * i + Vector3.right * j, 0, new int[0]);
                cells.Add(newCell);
            }
        }
    }
}
