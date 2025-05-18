using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CastleTiles
{
    public int totalTiles { get; private set; }
    private List<GameObject> tiles;

    public CastleTiles() {
        tiles = new List<GameObject>();
    }

    ~CastleTiles() {
        totalTiles = 0;
        tiles.Clear();
    }

    public void AddTile(GameObject tile) {
        tiles.Add(tile);
        totalTiles++;
    }

    public bool AllTilesCollapsed() {
        for (int i = 0; i < totalTiles; i++) {
            if (!tiles[i].GetComponent<Cell>().collapsed) {
                return false;
            }
        }
        return true;
    }
}
