using System;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Tile tile;
    public int cellIndex;
    public bool collapsed { get; private set; }
    [SerializeField] private int enthropy;

    [SerializeField] private List<Tile> possibleTiles;

    [Space(8)]
    public SurroundingTiles surroundingTiles;

    private GameObject currentTile;

    public bool perimeterTile;

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
        if (currentTile != null)
        {
            Destroy(currentTile);
        }
        currentTile = Instantiate(targetTile.TilePrefab, transform.position, Quaternion.Euler(0, tile.Orientation, 0), this.transform);
        if (tile.AttackTile) {
            OnAttackTileCreated?.Invoke();
        }
    }

    public Tile GetTile() {
        return tile;
    }

    public void SetRadomTile() {
        if(possibleTiles == null || enthropy == 0) {
            Collapse();
            SetTile(TileMenu.Neutral);
            return;
        }
        if(enthropy == 1) {
            Collapse();
            SetTile(possibleTiles[0]);
            return;
        }

        Tile tile = null;
        int index = 0;
        for (int i = 0; i < possibleTiles.Count; i++) {
            index += possibleTiles[i].weight;
        }

        int choice = UnityEngine.Random.Range(0, index);
        int currentTile = 0;
        int currentWeight = 0;
        currentWeight = possibleTiles[0].weight;
        for (int i = 0; i <= choice; i++) {
            if(i == choice) {
                tile = possibleTiles[currentTile];
                break;
            }
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
        if (possibleTiles != null) {
            enthropy = possibilities.Count;
            possibleTiles = possibilities;
        }
        else {
            enthropy = 0;
        }
    }

    public int GetEnthropy() {
        return enthropy;
    }
}

[System.Serializable]
public class SurroundingTiles {
    public Cell north;
    public Cell east;
    public Cell south;
    public Cell west;
    public Cell below;
}
