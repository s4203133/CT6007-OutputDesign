
public class CellCombinations {
    public int[] TopLeft { get; private set; }
    public int Top { get; private set; }
    public int[] TopRight { get; private set; }
    public int Left { get; private set; }
    public int Right { get; private set; }
    public int[] BottomLeft { get; private set; }
    public int Bottom { get; private set; }
    public int[] BottomRight { get; private set; }
    public int TopLeftInner { get; private set; }
    public int TopRightInner { get; private set; }
    public int BottomLeftInner { get; private set; }
    public int BottomRightInner { get; private set; }

    public int[] SingleTop { get; private set; }
    public int[] SingleRight { get; private set; }
    public int[] SingleLeft { get; private set; }
    public int[] SingleBottom { get; private set; }

    public int[] SingleEndTop { get; private set; }
    public int[] SingleEndRight { get; private set; }
    public int[] SingleEndLeft { get; private set; }
    public int[] SingleEndBottom { get; private set; }

    public CellCombinations(int args) {
        // Different combinations to represent where on the grid a tile is

        // Matrix Grid for reference:
        //        0   1   2
        //        3   -   4
        //        5   6   7

        TopLeft = new int[2] { 1, 3 };
        Top =  1;
        TopRight = new int[2] { 1, 4 };

        Left = 3;
        Right = 4;

        BottomLeft = new int[2] { 3, 6 };
        Bottom = 6;
        BottomRight = new int[2] { 4, 6 };

        TopLeftInner = 0;
        TopRightInner = 2;
        BottomLeftInner = 5;
        BottomRightInner = 7;


        SingleTop = new int[2] { 3, 4, };
        SingleRight = new int[2] { 1, 6, };
        SingleLeft = new int[2] { 1, 6, };
        SingleBottom = new int[2] { 3, 4, };

        SingleEndTop = new int[3] { 1, 3, 4, };
        SingleEndRight = new int[3] { 1, 4, 6, };
        SingleEndLeft = new int[3] { 1, 3, 6, };
        SingleEndBottom = new int[3] { 3, 4, 6, };
    }

    private bool SurroundingTilesMatch(int[] surroundingTiles, int targetTile) {
        // If any of the target id's is 0, then this combination has failed
        if (surroundingTiles[targetTile] == 0) {
            return false;
        }
        // Otherwise, this is the correct combination
        return true;
    }

    private bool SurroundingTilesMatch(int[] surroundingTiles, int[] targetTiles) {
        // For each entry in the provided combination array
        for (int value = 0; value < targetTiles.Length; value++) {
            // If the corresponding index in surrounding tiles value is 0, then this isn't the correct combination
            if (surroundingTiles[targetTiles[value]] == 0) {
                return false;
            }
        }
        // If all indexes in the surrounding tiles matched the combination, then this is the correct combination
        return true;
    }

    public Tile GetCorrectTile(int[] surroundingTiles) {
        // Calculate the correct type of tile based on the how many cells are surrounding the current cell
        Tile returnTile = CheckForSingularEnds(surroundingTiles);
        if(returnTile == null) {
            returnTile = CheckForSingularTiles(surroundingTiles);
        } if (returnTile == null) {
            returnTile = CheckForCorners(surroundingTiles);
        }
        if(returnTile == null) {
            returnTile = CheckForEdges(surroundingTiles);
        }
        if(returnTile == null) {
            returnTile = CheckForInnerCorners(surroundingTiles);
        }
        return returnTile;
    }

    private Tile CheckForSingularEnds(int[] tiles) {
        // Check if this the end piece of a singular strip of edges
        if (SurroundingTilesMatch(tiles, SingleEndTop)) {
            return TileMenu.SingleEndTop;
        }
        if (SurroundingTilesMatch(tiles, SingleEndLeft)) {
            return TileMenu.SingleEndLeft;
        }
        if (SurroundingTilesMatch(tiles, SingleEndRight)) {
            return TileMenu.SingleEndRight;
        }
        if (SurroundingTilesMatch(tiles, SingleEndBottom)) {
            return TileMenu.SingleEndBottom;
        }
        return null;
    }

    private Tile CheckForSingularTiles(int[] tiles) {
        // Check if this the wall piece of a singular strip of edges
        if (SurroundingTilesMatch(tiles, SingleTop)) {
            return TileMenu.SingleTop;
        }
        if (SurroundingTilesMatch(tiles, SingleRight)) {
            return TileMenu.SingleRight;
        }
        if (SurroundingTilesMatch(tiles, SingleLeft)) {
            return TileMenu.SingleLeft;
        }
        if (SurroundingTilesMatch(tiles, SingleBottom)) {
            return TileMenu.SingleBottom;
        }
        return null;
    }

    private Tile CheckForCorners(int[] tiles) {
        // Check if this an outer corner of the castle wall
        if (SurroundingTilesMatch(tiles, TopLeft)) {
            return TileMenu.TopLeftCorner;
        }
        if (SurroundingTilesMatch(tiles, TopRight)) {
            return TileMenu.TopRightCorner;
        }
        if (SurroundingTilesMatch(tiles, BottomLeft)) {
            return TileMenu.BottomLeftCorner;
        }
        if (SurroundingTilesMatch(tiles, BottomRight)) {
            return TileMenu.BottomRightCorner;
        }
        return null;
    }

    private Tile CheckForEdges(int[] tiles) {
        // Check if this a standard wall piece of the castle wall
        if (SurroundingTilesMatch(tiles, Left)) {
            return TileMenu.Empty;
        }
        if (SurroundingTilesMatch(tiles, Right)) {
            return TileMenu.Empty;
        }
        if (SurroundingTilesMatch(tiles, Top)) {
            return TileMenu.Empty;
        }
        if (SurroundingTilesMatch(tiles, Bottom)) {
            return TileMenu.Empty;
        }
        return null;
    }

    private Tile CheckForInnerCorners(int[] tiles) {
        // Check if this an interior corner piece of the castle wall
        if (SurroundingTilesMatch(tiles, TopLeftInner)) {
            return TileMenu.TopLeftInner;
        }
        if (SurroundingTilesMatch(tiles, TopRightInner)) {
            return TileMenu.TopRightInner;
        }
        if (SurroundingTilesMatch(tiles, BottomLeftInner)) {
            return TileMenu.BottomLeftInner;
        }
        if (SurroundingTilesMatch(tiles, BottomRightInner)) {
            return TileMenu.BottomRightInner;
        }
        return null;
    }
}