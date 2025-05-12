using UnityEngine;
using UnityEditor;
using ProceduralLevelDesign;
using UnityEditor.Experimental.GraphView;

[CustomEditor(typeof(GridTool))]
public class GridToolEditor : Editor
{
    [SerializeField] protected GridTool _gridTool;

    public override void OnInspectorGUI()
    {
        if (_gridTool == null)
        {
            _gridTool = (GridTool)target;
        }

        if (GUILayout.Button("Probing"))
        {
            //_gridTool.PlaceModule();
        }
    }
}
