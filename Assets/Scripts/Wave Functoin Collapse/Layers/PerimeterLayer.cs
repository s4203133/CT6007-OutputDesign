using UnityEngine;

public class PerimeterLayer : WaveFunctionCollapseLayer
{
    public override void Initialise(TileGrid grid, WaveFunctionCollapse wfc) {
        this.grid = grid;
    }

    public override bool Complete() {
        if (grid.AllPerimeterCellsCollapsed()) {
            return true;
        }
        return false;
    }

    public override bool TileCondition(Cell cell) {
        if (cell.perimeterTile) {
            return true;
        }
        return false;
    }
}
