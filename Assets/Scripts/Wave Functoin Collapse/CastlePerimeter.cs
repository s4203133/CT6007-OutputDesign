using System;
using UnityEngine;

[System.Serializable]
public class CastlePerimeter : CastleTiles  {

    public static Action<GameObject> OnPerimeterTileCreated;

    public CastlePerimeter() {
        OnPerimeterTileCreated += AddTile;
    }
}
