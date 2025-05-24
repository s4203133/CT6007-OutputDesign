using UnityEngine.SceneManagement;

public class PerimeterLayer : WaveFunctionCollapseLayer
{
    public override void Initialise(TileGrid grid, WaveFunctionCollapse wfc) {
        this.grid = grid;
        waveFunctionCollapse = wfc;
    }

    public override bool Complete() {
        // Once all cells marked as 'perimeter' have been collapsed, this layer is finished
        if (grid.AllPerimeterCellsCollapsed()) {
            return true;
        }
        return false;
    }

    public override bool TileCondition(Cell cell) {
        // If the number of iterations exceeds a large amount, the perimeter layer has overlapped into another layer
        // meaning this wave function collapse solution is unsuitable, so restart the scene to start again
        // (This shouldn't happen but it a safety measure)
        if(iterations > 100) {
            CastleTarget.OnSuccess?.Invoke();
            SceneManager.LoadScene(0);
        }
        // If this cell is on the perimeter of the castle, then it is allowed to be filled out while this layer is active
        if (cell.perimeterTile) {
            return true;
        }
        return false;
    }
}
