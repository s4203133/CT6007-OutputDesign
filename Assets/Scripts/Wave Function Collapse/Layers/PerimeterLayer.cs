using UnityEngine.SceneManagement;

public class PerimeterLayer : WaveFunctionCollapseLayer
{
    public override void Initialise(TileGrid grid, WaveFunctionCollapse wfc) {
        this.grid = grid;
        waveFunctionCollapse = wfc;
    }

    public override bool Complete() {
        if (grid.AllPerimeterCellsCollapsed()) {
            return true;
        }
        return false;
    }

    public override bool TileCondition(Cell cell) {
        if(iterations > 100) {
            CastleTarget.OnSuccess?.Invoke();
            SceneManager.LoadScene(0);
        }
        if (cell.perimeterTile) {
            return true;
        }
        return false;
    }
}
