using UnityEngine;

public class VerticalLayer : WaveFunctionCollapseLayer {

    public VerticalGrid verticalGrid;

    public override void Initialise(TileGrid grid, WaveFunctionCollapse wfc) {
        this.grid = grid;
        waveFunctionCollapse = wfc;

        verticalGrid = new VerticalGrid(grid.Tiles, grid.generator);
        verticalGrid.Generate();

        waveFunctionCollapse.MoveUpLayer();
        waveFunctionCollapse.SetGrid(verticalGrid.cells);
        grid.SetGrid(verticalGrid.cells);

        WaveFunctionCollapse.OnCellCollapsed += AddCellToList;
    }

    public override bool Complete() {
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
