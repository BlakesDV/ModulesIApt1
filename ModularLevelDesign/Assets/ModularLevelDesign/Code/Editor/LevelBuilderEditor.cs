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

            Dungeon dungeon = new Dungeon()
            {
                minX = 0,
                minY = 0,
                maxX = _levelBuilder.sizeX,
                maxY = _levelBuilder.sizeZ,
            };

            _levelBuilder.BinarySpacePartition(dungeon);
        }

        if (GUILayout.Button("Check Neighbours"))
        {
            _levelBuilder.CheckNeighbours();
        }

        if (GUILayout.Button("Create Hall"))
        {
            //_levelBuilder.SpawnHall();
        }

        if (GUILayout.Button("Delete Modules"))
        {
            _levelBuilder.DeleteAllModules();
        }

    }
}