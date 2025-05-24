using System;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool debugThisTile;

    private Tile tile;
    public int cellIndex;
    private int enthropy;
    public bool collapsed { get; private set; }

    private List<Tile> possibleTiles;

    [Space(8)]
    public SurroundingTiles surroundingTiles;

    private GameObject currentTile;

    [HideInInspector] public bool perimeterTile;

    public static Action OnAttackTileCreated;

    public void Init() {
        surroundingTiles = new SurroundingTiles();
        enthropy = TileMenu.NumberOfTiles;
    }

    public void SetOrientation() {
        transform.rotation = Quaternion.Euler(0, tile.Orientation, 0);
    }

    public void SetTile(Tile targetTile) {
        tile = targetTile;
        // If a tile alreayd exists, destroy it
        if (currentTile != null)
        {
            Destroy(currentTile);
        }
        currentTile = Instantiate(targetTile.TilePrefab, transform.position, Quaternion.Euler(0, tile.Orientation, 0), this.transform);

        // Register if an attack tile was created
        if (tile.AttackTile) {
            OnAttackTileCreated?.Invoke();
        }
    }

    public Tile GetTile() {
        return tile;
    }

    public void SetRadomTile() {
        // If there are no possible tiles, collapse this cell will a neutral tile
        if (possibleTiles == null || enthropy == 0) {
            Collapse();
            SetTile(TileMenu.Neutral);
            return;
        }
        // If there is only one possible tile, collapse this cell with it
        if (enthropy == 1) {
            Collapse();
            SetTile(possibleTiles[0]);
            return;
        }

        // Add up all the weights of the possible tiles this cell can be collapsed as
        Tile tile = null;
        int index = 0;
        for (int i = 0; i < possibleTiles.Count; i++) {
            index += possibleTiles[i].weight;
        }

        // Genarate a random number for choosing a tile
        int choice = UnityEngine.Random.Range(0, index);
        int currentTile = 0;
        int currentWeight = 0;
        currentWeight = possibleTiles[0].weight;
        // Increment 'i' until it reaches the random number
        for (int i = 0; i <= choice; i++) {
            if(i == choice) {
                tile = possibleTiles[currentTile];
                break;
            }
            // If 'i' has surpassed the wieght of the current tile, move onto the next tile and update the current weight
            if (i > currentWeight) {
                currentTile++;
                currentWeight += possibleTiles[currentTile].weight;
            }
        }
        Collapse();
        SetTile(tile);
    }

    public void Collapse() {
        collapsed = true;
        enthropy = 0;
    }

    public void SetIndex(int index) {
        cellIndex = index;
    }

    public void SetPossibilities(List<Tile> possibilities) {
        // If this cell has possible tiles, set the enthropy and possible tiles list
        if (possibilities != null) {
            enthropy = possibilities.Count;
            possibleTiles = possibilities;
        }
        // If there are no possible tiles, set the enthropy to 0
        else {
            enthropy = 0;
        }
    }

    public int GetEnthropy() {
        return enthropy;
    }
}

[Serializable]
public class SurroundingTiles {
    public Cell north;
    public Cell east;
    public Cell south;
    public Cell west;
    public Cell below;
}
