using UnityEngine;
using System.Reflection;
using System;

public class CastleGenerator : MonoBehaviour
{
    [SerializeField] private CastleData castleData;

    [Space(10)]
    [SerializeField] private BinarySpacePartitioning binarySpaceParitioning;
    [Space(10)]
    [SerializeField] private TileGrid tileGrid;
    [Space(10)]
    [SerializeField] private WaveFunctionCollapse waveFunctionCollapse;

    [Space(10)]
    [SerializeField] private TileMenu tileMenu;

    private void Start() {
        GenerateMap();
    }

    public void GenerateMap() {
        GenerateFloorMap();
        Invoke("GenerateTiles", 0.1f);
        Invoke("StartWFC", 0.1f);
    }

    private void GenerateFloorMap() {
        // Initialise data before creating the floor map for the castle to be placed upon
        tileMenu.Initialise();
        binarySpaceParitioning.Initialise(castleData);
        tileGrid.Initialise(castleData);

        binarySpaceParitioning.CreateFloorMap();
    }

    private void GenerateTiles() {
        tileGrid.Generate();
        waveFunctionCollapse.Initialise(tileGrid);
    }

    private void StartWFC() {
        StartCoroutine(waveFunctionCollapse.WFC());
    }
}

[System.Serializable]
public struct CastleData {
    public int width;
    public int length;
    [Space(10)]
    public int minRoomWidth;
    public int minRoomLength;
}