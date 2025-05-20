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
        tileMenu.Initialise();

        binarySpaceParitioning.Initialise(castleData);
        tileGrid.Initialise(castleData);

        binarySpaceParitioning.CreateFloorMap();
        Invoke("GenerateTiles", 0.1f);

        waveFunctionCollapse.Initialise(tileGrid);
        Invoke("StartWFC", 0.1f);
    }

    public void GenerateMap() {
        if (Application.isPlaying) {
            ClearConsole();
            binarySpaceParitioning.ClearMap();
            binarySpaceParitioning.CreateFloorMap();
            tileGrid.ClearGrid();
            Invoke("GenerateTiles", 0.1f);
            waveFunctionCollapse.Initialise(tileGrid);
            Invoke("StartWFC", 0.1f);
        }
        else {
            Debug.LogWarning("Please Enter Play mode before calling this function.");
        }
    }

    private void GenerateTiles() {
        tileGrid.Generate();
    }

    private void StartWFC() {
        StartCoroutine(waveFunctionCollapse.WFC());
    }

    public void ClearConsole() {
        Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        Type type = assembly.GetType("UnityEditor.LogEntries");
        MethodInfo methodInfo = type.GetMethod("Clear");
        methodInfo.Invoke(new object(), null);
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