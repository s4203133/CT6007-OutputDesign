using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CastleGenerator))]
public class CastleGeneratorEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        CastleGenerator generator = (CastleGenerator)target;

        EditorGUILayout.Space(15);

        if(GUILayout.Button("Generate New Map")) {
            generator.GenerateMap();
        }
    }
}
