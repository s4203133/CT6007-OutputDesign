using UnityEngine;

[CreateAssetMenu(menuName = "Create Tile Menu", fileName = "Tile Menu")]
public class TileMenu : ScriptableObject
{
    private static TileMenu instance;

    public void Initialise() {
        instance = this;
    }

    [SerializeField] private int numberOfTiles;

    [Space(15)]
    [Header("TILES")]
    [SerializeField] private Tile empty;
    [SerializeField] private Tile neutralTower;

    [Space(15)]
    [Header("EDGES")]
    [SerializeField] private Tile leftEdge;
    [SerializeField] private Tile rightEdge;
    [SerializeField] private Tile topEdge;
    [SerializeField] private Tile bottomEdge;

    [Space(15)]
    [Header("CORNERS")]
    [SerializeField] private Tile topLeftCorner;
    [SerializeField] private Tile topRightCorner;
    [SerializeField] private Tile bottomLeftCorner;
    [SerializeField] private Tile bottomRightCorner;

    [Space(15)]
    [Header("INNER CORNERS")]
    [SerializeField] private Tile topLeftInner;
    [SerializeField] private Tile topRightInner;
    [SerializeField] private Tile bottomLeftInner;
    [SerializeField] private Tile bottomRightInner;

    [Space(15)]
    [Header("SINGLE EDGES")]
    [SerializeField] private Tile singleTop;
    [SerializeField] private Tile singleRight;
    [SerializeField] private Tile singleLeft;
    [SerializeField] private Tile singleBottom;

    [Space(15)]
    [Header("SINGLE ENDS")]
    [SerializeField] private Tile singleEndTop;
    [SerializeField] private Tile singleEndRight;
    [SerializeField] private Tile singleEndLeft;
    [SerializeField] private Tile singleEndBottom;

    public static int NumberOfTiles => instance.numberOfTiles;

    public static Tile Empty => instance.empty;
    public static Tile Neutral => instance.neutralTower;

    public static Tile LeftEdge => instance.leftEdge;
    public static Tile RightEdge => instance.rightEdge;
    public static Tile TopEdge => instance.topEdge;
    public static Tile BottomEdge => instance.bottomEdge;

    public static Tile TopLeftCorner => instance.topLeftCorner;
    public static Tile TopRightCorner => instance.topRightCorner;
    public static Tile BottomLeftCorner => instance.bottomLeftCorner;
    public static Tile BottomRightCorner => instance.bottomRightCorner;

    public static Tile TopLeftInner => instance.topLeftInner;
    public static Tile TopRightInner => instance.topRightInner;
    public static Tile BottomLeftInner => instance.bottomLeftInner;
    public static Tile BottomRightInner => instance.bottomRightInner;

    public static Tile SingleTop => instance.singleTop;
    public static Tile SingleRight => instance.singleRight;
    public static Tile SingleLeft => instance.singleLeft;
    public static Tile SingleBottom => instance.singleBottom;

    public static Tile SingleEndTop => instance.singleEndTop;
    public static Tile SingleEndRight => instance.singleEndRight;
    public static Tile SingleEndLeft => instance.singleEndLeft;
    public static Tile SingleEndBottom => instance.singleEndBottom;
}
