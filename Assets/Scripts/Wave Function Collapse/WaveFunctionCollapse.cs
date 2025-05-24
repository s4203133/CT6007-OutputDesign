using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveFunctionCollapse
{
    private TileGrid grid;
    private List<GameObject> tiles;

    private bool completed;

    [SerializeField] private int maxHeight;
    private int height;

    [Space(5)]
    [SerializeField] private int attackSpendingPoints;
    private int attackPoints;

    [SerializeField] private WaveFunctionCollapseLayer[] layers;
    private WaveFunctionCollapseLayer currentLayer;
    private int currentLayerIndex;

    public static Action<GameObject> OnCellCollapsed;
    public static Action OnFinished;

    public void Initialise(TileGrid tileGrid) {
        grid = tileGrid;
        tiles = grid.Tiles;
        height = 1;

        currentLayer = layers[0];
        currentLayer.Initialise(grid, this);

        attackPoints = attackSpendingPoints;
        Cell.OnAttackTileCreated += SpendAttackPoint;
    }

    public IEnumerator WFC() {
        completed = false;
        while (!completed) {
            // Calculate the enthropy of each tile, find the one with the lowest value, and collapse it with a random tile
            CalculateEnthropy();
            Cell cell = GetCellWithLowestEnthropy();
            CollapseCell(cell);
            currentLayer.AddIteration();

            // If the layer has finished, move onto the next one
            if (currentLayer.Complete()) {
                currentLayerIndex++;
                // If this is the final layer, finish the wave function collapse and return
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
            // For each cell in the grid, get the surrounding tiles and check what tiles can be placed next to them to determine how many
            // possible tiles can occpy this current cell
            Cell currentCell = tiles[i].GetComponent<Cell>();
            if (currentCell.debugThisTile) {
                Debug.Log("Break!");
            }

            // If height is 1 then this cell is on the floormap of the castle
            if (height == 1) {
                currentCell.surroundingTiles = grid.GetSurroundingTiles(i);
            }
            // Otherwise this is a cell on a vertical layer
            else {
                currentCell.surroundingTiles = grid.GetSurroundingTiles(currentCell);
            }

            // Calculate the possible tiles that can occpy this current cell space
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

        // For each cell, determine which one has the lowest enthopy
        for(int i = 0; i < count; i++) {
            Cell currentCell = tiles[i].GetComponent<Cell>();
            // If this cell has already been collapsed, or the current layers condition doesn't match this cell, don't include in the calculation
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

        // If no cells were found, this is an invalid wave function collapse solution
        // (This shouldn't happen but is a safety measure)
        if(lowestEnthropyCells.Count == 0) {
            CastleTarget.OnFail?.Invoke();
            return null;
        }
        // If one cell had the lowest enthropy, return it
        else if (lowestEnthropyCells.Count == 1) {
            return lowestEnthropyCells[0];
        }
        // If more than one cell share the least enthropy value, return a random one
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

    private void SpendAttackPoint() {
        attackPoints--;
    }

    public bool SpentAllAttackPoints => attackPoints <= 0;
}
