using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Tile", menuName = "Create New Tile")]
public class Tile : ScriptableObject
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int orientation;
    public int weight; 

    [Space(10)]
    [SerializeField] private List<Tile> northTiles;
    [SerializeField] private List<Tile> southTiles;

    [Space(10)]
    [SerializeField] private List<Tile> eastTiles;
    [SerializeField] private List<Tile> westTiles;

    [Space(10)]
    [SerializeField] private List<Tile> aboveTiles;

    [SerializeField] private bool startCollapsed;

    // Public Access
    public GameObject TilePrefab => tilePrefab;
    public int Orientation => orientation;
    public List<Tile> North => northTiles;
    public List<Tile> South => southTiles;
    public List<Tile> East => eastTiles;
    public List<Tile> West => westTiles;
    public List<Tile> Above => aboveTiles;
    public bool StartCollapsed => startCollapsed;   
}
