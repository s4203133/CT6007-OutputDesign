using UnityEngine;

public class InnerLayer : WaveFunctionCollapseLayer
{
    public override void Initialise(TileGrid grid, WaveFunctionCollapse wfc) {
        this.grid = grid;
        waveFunctionCollapse = wfc;
    }

    public override bool Complete() {
        return grid.AllInteriorCellsCollapsed();
    }

    public override bool TileCondition(Cell cell) {
        return true;
    }
}
