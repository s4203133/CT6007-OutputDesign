using System;
using UnityEngine;

[System.Serializable]
public class CastleInterior : CastleTiles {

    public static Action<GameObject> OnInteriorTileCreated;

    public CastleInterior() {
        OnInteriorTileCreated += AddTile;
    }
}
