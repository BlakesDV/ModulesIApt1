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

        public void CreateGrid()
        {
            Vector3 startPos = transform.position;
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    Vector3 modulePos = startPos + new Vector3(x * cellsize, 0, z * cellsize);
                    GameObject module = Instantiate(modulePrefab, modulePos, Quaternion.identity);
                    module.transform.parent = this.transform;

                    Module moduleComponent = module.GetComponent<Module>();
                    moduleComponent.SetPosition(new Vector3(x, 0, z));

                    grid[x, z] = module;
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    if (grid[x, z] != null)
                    {
                        grid[x, z].GetComponent<Module>().UpdateModules();
                    }
                }
            }
        }

        public GameObject GetModuleAt(int x, int z)
        {
            if (x >= 0 && z >= 0 && x < width && z < height)
            {
                return grid[x, z];
            }
            return null;
        }

        public void DisableModuleAt(int x, int z)
        {
            GameObject module = GetModuleAt(x, z);
            if (module != null)
            {
                module.SetActive(false);
            }
        }
    }
}
