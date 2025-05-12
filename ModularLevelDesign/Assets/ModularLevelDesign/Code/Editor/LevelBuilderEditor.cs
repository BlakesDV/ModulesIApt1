using UnityEngine;
using UnityEditor;
using ProceduralLevelDesign;
using UnityEditor.Experimental.GraphView;

[CustomEditor(typeof(LevelBuilder))]
public class LevelBuilderEditor : Editor
{
    [SerializeField] protected LevelBuilder _levelBuilder;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (_levelBuilder == null)
        {
            _levelBuilder = (LevelBuilder)target;
        }

        if (GUILayout.Button("Probing"))
        {
            _levelBuilder.CreateGrid();
        }
        if (GUILayout.Button("BSP"))
        {
            _levelBuilder.BPS();
        }
    }
}