using UnityEngine;

public class VerticalLayer : WaveFunctionCollapseLayer {

    public VerticalGrid verticalGrid;

    public override void Initialise(TileGrid grid, WaveFunctionCollapse wfc) {
        this.grid = grid;
        waveFunctionCollapse = wfc;

        // Generate a vertical grid to place towers
        verticalGrid = new VerticalGrid(grid.Tiles, grid.generator);
        verticalGrid.Generate();

        // Move up a vertical layer to start placing tiles ABOVE the ones already collapsed
        waveFunctionCollapse.MoveUpLayer();
        waveFunctionCollapse.SetGrid(verticalGrid.cells);
        grid.SetGrid(verticalGrid.cells);

        WaveFunctionCollapse.OnCellCollapsed += AddCellToList;
    }

    public override bool Complete() {
        // If all possible cells have been collapsed, then this layer is finished
        for(int i = 0; i < verticalGrid.cells.Count; i++) {
            if (!verticalGrid.cells[i].GetComponent<Cell>().collapsed) {
                return false;
            }
        }
        return true;
    }

    private void AddCellToList(GameObject newCell) {

    }

    public override bool TileCondition(Cell cell) {
        return true;
    }
}
