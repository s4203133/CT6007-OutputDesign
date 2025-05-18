using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileRenderer))]
public class TileRendererEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        TileRenderer renderer = (TileRenderer)target;

        EditorGUILayout.Space(2);

        if (GUILayout.Button("Quick Render")) {
            Cell cell = renderer.GetComponent<Cell>();
            if (cell.GetTile() != null) {
                //renderer.Render(cell.GetTile());
                cell.SetOrientation();
            }
            else {
                Debug.Log("Please assign a Tile in the 'Cell' component before rendering", target);
            }
        }
    }
}
