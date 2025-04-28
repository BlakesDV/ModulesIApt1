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

        public Vector2Int GridWorldPos(Vector3 worldPos)
        {
            int x = Mathf.RoundToInt(worldPos.x / cellsize);
            int y = Mathf.RoundToInt(worldPos.y / cellsize);
            return new Vector2Int(x, y);
        }

        public void PlaceModule(Vector3 worldPos)
        {
            Vector2Int gridPos = GridWorldPos(worldPos);
            //If for placing the module inside the grid position
            if (IsInsideGrid(gridPos) && grid[gridPos.x, gridPos.y] == null)
            {
                Vector3 placePos = new Vector3(gridPos.x * cellsize, 0, gridPos.y * cellsize);
                GameObject module = Instantiate(modulePrefab, placePos, Quaternion.identity);
                grid[gridPos.x, gridPos.y] = module;
                module.GetComponent<Module>().SetPosition(gridPos);
                UpdateNeighbours(gridPos);
            }
        }

        //Create function for checking if the module is inside the grid
        private bool IsInsideGrid(Vector2Int gridPos)
        {
            return gridPos.x >= 0 && gridPos.y >= 0 && gridPos.x < width && gridPos.y < height;
        }

        private void UpdateNeighbours(Vector2Int center)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    Vector2Int pos = center + new Vector2Int(dx, dy);
                    if (IsInsideGrid(pos) && grid[pos.x, pos.y] != null)
                    {
                        grid[pos.x, pos.y].GetComponent<Module>().UpdateModules();
                    }
                }
            }
        }
    }
}
