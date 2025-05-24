using UnityEngine;

public class TileStats : MonoBehaviour
{
    [Header("BASE STATS")]
    [SerializeField] private CastleStats baseStats;
    public CastleStats BaseStats => baseStats;

    [Header("DOORS")]
    [SerializeField] private Tile[] doors;

    [Header("WEAPONS")]
    [SerializeField] private Tile[] cannons;
    [SerializeField] private Tile[] catapults;
    [SerializeField] private Tile[] crossbows;

    [Header("STRONG WALLS")]
    [SerializeField] private Tile[] strongWalls;

    private void AdjustWeights(ref Tile[] tiles, int[] amounts) {
        // Set the weights to the values passed in
        tiles[0].weight = amounts[0];
        tiles[1].weight = amounts[1];
        tiles[2].weight = amounts[2];
        tiles[3].weight = amounts[3];
    }

    public void AdjustDoorwayWeights(int[] amounts) => AdjustWeights(ref doors, amounts);

    public void AdjustCannonWeights(int[] amounts) => AdjustWeights(ref cannons, amounts);

    public void AdjustCatapultWeights(int[] amounts) => AdjustWeights(ref catapults, amounts);

    public void AdjustCrossbowWeights(int[] amounts) => AdjustWeights(ref crossbows, amounts);

    public void AdjustStrongWallWeights(int[] amounts) => AdjustWeights(ref strongWalls, amounts);
}
