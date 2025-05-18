using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class WaveFunctionCollapse
{
    private TileGrid grid;
    private List<GameObject> tiles;

    public bool completed;
    public int iterations;

    [Range(0f, 0.1f)]
    public float speed;
    private WaitForSeconds delay;

    [SerializeField] private int maxHeight;
    private int height;

    [SerializeField] private WaveFunctionCollapseLayer[] layers;
    private WaveFunctionCollapseLayer currentLayer;
    private int currentLayerIndex;

    public static Action<GameObject> OnCellCollapsed;
    public static Action OnFinished;

    public void Initialise(TileGrid tileGrid) {
        grid = tileGrid;
        tiles = grid.Tiles;
        delay = new WaitForSeconds(speed);
        height = 1;
        currentLayer = layers[0];
        currentLayer.Initialise(grid, this);
    }

    public IEnumerator WFC() {
        completed = false;
        while (!completed) {
            CalculateEnthropy();
            Cell cell = GetCellWithLowestEnthropy();
            CollapseCell(cell);
            iterations++;

            if (currentLayer.Complete()) {
                currentLayerIndex++;
                if (currentLayerIndex >= layers.Length) {
                    completed = true;
                    OnFinished?.Invoke();
                    break;
                }
                currentLayer = layers[currentLayerIndex];
                currentLayer.Initialise(grid, this);
            }
            yield return null;
        }
    }

    private void CalculateEnthropy() {
        int count = tiles.Count;
        for(int i = 0; i < count; i++) {
            Cell currentCell = tiles[i].GetComponent<Cell>();

            if (height == 1) {
                currentCell.surroundingTiles = grid.GetSurroundingTiles(i);
            }
            else {
                currentCell.surroundingTiles = grid.GetSurroundingTiles(currentCell);
            }
            List<Tile> possibileTiles = CalculatePossibilities(currentCell.surroundingTiles);
            currentCell.SetPossibilities(possibileTiles);
        }
    }

    private List<Tile> CalculatePossibilities(SurroundingTiles surroundingTiles) {
        return currentLayer.GetPossibleTiles(surroundingTiles);
    }

    private Cell GetCellWithLowestEnthropy() {
        int lowestEnthropy = 100;
        int count = tiles.Count;

        List<Cell> lowestEnthropyCells = new List<Cell>();

        for(int i = 0; i < count; i++) {
            Cell currentCell = tiles[i].GetComponent<Cell>();
            if (currentCell.collapsed || !layers[currentLayerIndex].TileCondition(currentCell)) {
                continue;
            }
            if(currentCell.GetEnthropy() <= lowestEnthropy) {
                // If there is a new LOWER enthropy (but not equal to the current one), clear the list for the cells with the new lower value
                if (currentCell.GetEnthropy() < lowestEnthropy) {
                    lowestEnthropyCells.Clear();
                }
                lowestEnthropy = currentCell.GetEnthropy();
                lowestEnthropyCells.Add(currentCell);
            }
        }

        if(lowestEnthropyCells.Count == 0) {
            CastleTarget.OnFail?.Invoke();
            return null;
        }
        else if (lowestEnthropyCells.Count == 1) {
            return lowestEnthropyCells[0];
        }
        else {
            int randomCell = UnityEngine.Random.Range(0, lowestEnthropyCells.Count -1);
            return lowestEnthropyCells[randomCell];
        }
    }

    private void CollapseCell(Cell cell) {
        OnCellCollapsed?.Invoke(cell.gameObject);
        cell.SetRadomTile();
    }

    public void MoveUpLayer() {
        height++;
    }

    public void SetGrid(List<GameObject> newGrid) {
        tiles = newGrid;
    }
}
