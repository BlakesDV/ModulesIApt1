using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using static ProceduralLevelDesign.Module;
using UnityEngine.UIElements;

namespace ProceduralLevelDesign
{
    #region Interfaces
    public interface ILevelEditor
    {
        public void ClearLevel();
        public void DeleteModule(Vector2 value);
        public void CreateModule(Vector2 value);
    }
    #endregion Interfaces

    #region Structs
    [SerializeField]
    public struct Dungeon
    {
        public int minX;
        public int minY;
        public int maxX;
        public int maxY;

        public bool isSliceableOnX;
        public bool isSliceableOnY;

        public int Width()
        {
            return maxX - minX;
        }
        public int Height()
        {
            return maxY - minY;
        }
    }

    #endregion

    public class LevelBuilder : MonoBehaviour, ILevelEditor
    {
        #region Parameters

        [SerializeField] GameObject _modulePrefab;
        [SerializeField] protected int minDungeonX;
        [SerializeField] protected int minDungeonY;
        public int sizeX = 1, sizeZ = 1;
        [SerializeField] private Vector3 gridPos;

        #endregion Parameters

        #region InternalData

        [SerializeField] public List<Module> _allModulesInScene;
        [SerializeField] protected Module[,] _bidimentionalGrid;

        #endregion InternalData

        #region RuntimeVars

        protected Ray rayFromSceneCamera;
        protected RaycastHit raycastHit;
        protected GameObject moduleInstance;
        protected Vector3 modulePosition;


        #endregion RuntimeVars

        #region InterfaceMethods

        public void ClearLevel()
        {
            foreach (Module module in transform.GetComponentsInChildren<Module>())
            {
                DestroyImmediate(module.gameObject);
            }
            _allModulesInScene.Clear();
        }

        public void DeleteModule(Vector2 value)
        {
            rayFromSceneCamera = HandleUtility.GUIPointToWorldRay(value); //Camera.main.ScreenPointToRay(value)
            Debug.DrawRay(rayFromSceneCamera.origin, rayFromSceneCamera.direction * 10000f, Color.red, 5f);
            if (Physics.Raycast(rayFromSceneCamera, out raycastHit, 10000f))
            {
                if (raycastHit.collider.gameObject.layer != 3)
                {
                    moduleInstance = raycastHit.collider.transform.parent.parent.gameObject;
                    _allModulesInScene.Remove(moduleInstance.GetComponent<Module>());
                    DestroyImmediate(moduleInstance);
                }
            }
            foreach (Module module in _allModulesInScene)
            {
                module.GetComponent<Module>().UpdateModules();
            }
        }

        public void CreateModule(Vector2 value)
        {
            rayFromSceneCamera = HandleUtility.GUIPointToWorldRay(value); //Camera.main.ScreenPointToRay(value)
            Debug.DrawRay(rayFromSceneCamera.origin, rayFromSceneCamera.direction * 10000f, Color.green, 5f);
            if (Physics.Raycast(rayFromSceneCamera, out raycastHit, 10000f))
            {
                if (raycastHit.collider.gameObject.layer == 6) //Layer -> Layout
                {
                    moduleInstance = Instantiate(_modulePrefab);
                    moduleInstance.transform.parent = transform;
                    modulePosition = raycastHit.point;
                    modulePosition.x = (int)modulePosition.x;
                    modulePosition.y = (int)modulePosition.y;
                    modulePosition.z = (int)modulePosition.z;
                    moduleInstance.transform.position = modulePosition;
                    moduleInstance.GetComponent<Module>().GridPos = modulePosition;

                    moduleInstance.GetComponent<Module>().SetLevelBuilder = this;
                    _allModulesInScene.Add(moduleInstance.GetComponent<Module>());

                    foreach (Module module in _allModulesInScene)
                    {
                        module.GetComponent<Module>().UpdateModules();
                    }
                }
            }
        }
        #endregion InterfaceMethods

        //Create function for checking module sides
        public bool ModuleSides(int x, int z)
        {
            foreach (Module module in _allModulesInScene)
            {
                if ((int)module.GridPos.x == x && (int)module.GridPos.z == z)
                {
                    return module.IsActive;
                }
            }
            return false;
        }

        public Module GetModuleAt(int x, int z)
        {
            foreach (Module module in _allModulesInScene)
            {
                if ((int)module.GridPos.x == x && (int)module.GridPos.z == z)
                {
                    return module;
                }
            }
            return null;
        }

        public void CreateGrid()
        {
            Vector3 startPos = transform.position - new Vector3(0, 0, 0);
            _bidimentionalGrid = new Module[sizeX, sizeZ];
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    Vector3 modulePos = startPos + new Vector3(x, 0, z);
                    moduleInstance = Instantiate(_modulePrefab, modulePos, Quaternion.identity);
                    moduleInstance.transform.parent = this.transform.GetChild(0);
                    //moduleInstance.transform.position = new Vector3(x, 0, z);
                    //moduleInstance.GetComponent<Module>().SetPosition(gridPos);
                    moduleInstance.GetComponent<Module>().levelBuilder = this;
                    moduleInstance.GetComponent<Module>().SetPosition(modulePos);
                    _allModulesInScene.Add(moduleInstance.GetComponent<Module>());

                    //_bidimentionalGrid[x, z] = moduleInstance.GetComponent<Module>();
                    //_allModulesInScene.Add(moduleInstance.GetComponent<Module>());
                }
            }
            CheckNeighbours();
        }

        public void DeleteAllModules()
        {
            foreach (Module module in _allModulesInScene)
            {
                module.gameObject.SetActive(true);
            }

            foreach (Module module in transform.GetComponentsInChildren<Module>())
            {
                DestroyImmediate(module.gameObject);
            }
            _allModulesInScene.Clear();
        }

        public void DisableAllModules()
        {
            foreach (Module module in transform.GetChild(0).GetComponentsInChildren<Module>())
            {
                //module.VisibilityOfTheModule(Visibility.OFF);
            }
        }

        public Module ModuleAtMatrixPosition(int x, int z)
        {
            if (x < 0 || z < 0) { return null; }
            if (x >= sizeX || z >= sizeZ) { return null; }
            return _bidimentionalGrid[x, z];
        }

        public void CheckNeighbours()
        {
            Debug.Log("Checking Neighbours");
            foreach (Module module in _allModulesInScene)
            {
                if (module.IsActive)
                {
                    module.UpdateModules();
                }
            }
        }

        //Create void binary space partition
        public void BinarySpacePartition(Dungeon dungeon)
        {
            if (dungeon.Width() > minDungeonX * 2)
            {
                dungeon.isSliceableOnX = true;
            }
            if (dungeon.Height() > minDungeonY * 2)
            {
                dungeon.isSliceableOnY = true;
            }
            if (!dungeon.isSliceableOnY && !dungeon.isSliceableOnX)
            {
                return;
            }

            if (dungeon.isSliceableOnY && dungeon.isSliceableOnX)
            {
                dungeon.isSliceableOnX = Random.Range(0, 2) == 0;
                dungeon.isSliceableOnY = !dungeon.isSliceableOnX;
            }

            //Cortes en en Y sobre eje X
            if (dungeon.isSliceableOnX && !dungeon.isSliceableOnY)
            {
                int cutter = Random.Range(dungeon.minX + minDungeonX + 1, dungeon.maxX - minDungeonX - 1);

                //for (int i = dungeon.minY; i <= dungeon.maxY; i++)
                //{
                //    GetModuleAt(cutter, i)?.gameObject.SetActive(false);
                //}
                foreach (Module module in _allModulesInScene)
                {
                    if (module.GridPos.x == cutter)
                    {
                        if (module.GridPos.z >= dungeon.minY && module.GridPos.z <= dungeon.maxY)
                        {
                            module.SetActive(false);
                        }
                    }
                }

                Dungeon DungeonA = new Dungeon()
                {
                    minX = dungeon.minX,
                    maxX = cutter - 1,
                    minY = dungeon.minY,
                    maxY = dungeon.maxY
                };

                Dungeon DungeonB = new Dungeon()
                {
                    minX = cutter + 1,
                    maxX = dungeon.maxX,
                    minY = dungeon.minY,
                    maxY = dungeon.maxY
                };

                BinarySpacePartition(DungeonA);
                BinarySpacePartition(DungeonB);
            }

            //Cortes en en X sobre eje Y
            else if (!dungeon.isSliceableOnX && dungeon.isSliceableOnY)
            {
                int cutter = Random.Range(dungeon.minY + minDungeonY + 1, dungeon.maxY - minDungeonY - 1);

                //for (int i = dungeon.minY; i <= dungeon.maxY; i++)
                //{
                //    GetModuleAt(cutter, i)?.gameObject.SetActive(false);
                //}
                foreach (Module module in _allModulesInScene)
                {
                    if (module.GridPos.z == cutter)
                    {
                        if (module.GridPos.x >= dungeon.minX && module.GridPos.x <= dungeon.maxX)
                        {
                            module.SetActive(false);
                        }
                    }
                }

                Dungeon DungeonA = new Dungeon()
                {
                    minX = dungeon.minX,
                    maxX = dungeon.maxX,
                    minY = dungeon.minY,
                    maxY = cutter - 1,
                };

                Dungeon DungeonB = new Dungeon()
                {
                    minX = dungeon.minX,
                    maxX = dungeon.maxX,
                    minY = cutter + 1,
                    maxY = dungeon.maxY
                };

                BinarySpacePartition(DungeonA);
                BinarySpacePartition(DungeonB);
            }
        }
    }
}