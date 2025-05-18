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

    public void AdjustDoorwayWeights(int amount, int amount2, int amount3, int amount4) => AdjustWeights(ref doors, amount, amount2, amount3, amount4);

    public void AdjustCannonWeights(int amount, int amount2, int amount3, int amount4) => AdjustWeights(ref cannons, amount, amount2, amount3, amount4);

    public void AdjustCatapultWeights(int amount, int amount2, int amount3, int amount4) => AdjustWeights(ref catapults, amount, amount2, amount3, amount4);

    public void AdjustCrossbowWeights(int amount, int amount2, int amount3, int amount4) => AdjustWeights(ref crossbows, amount, amount2, amount3, amount4);

    public void AdjustStrongWallWeights(int amount, int amount2, int amount3, int amount4) => AdjustWeights(ref strongWalls, amount, amount2, amount3, amount4);

    private void AdjustWeights(ref Tile[] tiles, int amount, int amount2, int amount3, int amount4) {
        tiles[0].weight = amount;
        tiles[1].weight = amount2;
        tiles[2].weight = amount3;
        tiles[3].weight = amount4;
    }
}
