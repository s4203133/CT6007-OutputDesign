using System;
using UnityEngine;

[Serializable]
public class CastleInterior : CastleTiles {

    public static Action<GameObject> OnInteriorTileCreated;

    public CastleInterior() {
        OnInteriorTileCreated += AddTile;
    }
}
