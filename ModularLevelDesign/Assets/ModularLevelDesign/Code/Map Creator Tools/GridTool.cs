using ProceduralLevelDesign;
using System.Reflection;
using UnityEngine;

namespace ProceduralLevelDesign
{
    public class GridTool : MonoBehaviour
    {
        public static GridTool Instance;
        public int width = 10, height = 10;
        public float cellsize = 1f;
        public GameObject modulePrefab;
        private GameObject[,] grid;

        private void Awake()
        {
            Instance = this;
            grid = new GameObject[width, height];
        }

        public Vector3 GridWorldPos(Vector3 worldPos)
        {
            int x = Mathf.RoundToInt(worldPos.x / cellsize);
            int y = Mathf.RoundToInt(worldPos.y / cellsize);
            return new Vector3(x, 0, y);
        }

        public void PlaceModule(Vector3 worldPos)
        {
            Vector3 gridPos = GridWorldPos(worldPos);
            //If for placing the module inside the grid position
            if (IsInsideGrid(gridPos) && grid[(int)gridPos.x, (int)gridPos.y] == null)
            {
                Vector3 placePos = new Vector3((int)gridPos.x * cellsize, 0, (int)gridPos.z * cellsize);
                GameObject module = Instantiate(modulePrefab, placePos, Quaternion.identity);
                grid[(int)gridPos.x, (int)gridPos.y] = module;
                module.GetComponent<Module>().SetPosition(gridPos);
                UpdateNeighbours(gridPos);
            }
        }

        //Create function for checking if the module is inside the grid
        private bool IsInsideGrid(Vector3 gridPos)
        {
            return gridPos.x >= 0 && gridPos.z >= 0 && gridPos.x < width && gridPos.z < height;
        }

        private void UpdateNeighbours(Vector3 center)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    Vector3 pos = center + new Vector3(dx, 0, dy);
                    if (IsInsideGrid(pos) && grid[(int)pos.x, (int)pos.y] != null)
                    {
                        grid[(int)pos.x, (int)pos.y].GetComponent<Module>().UpdateModules();
                    }
                }
            }
        }
    }
}
