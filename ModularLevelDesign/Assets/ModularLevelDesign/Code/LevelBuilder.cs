using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ProceduralLevelDesign {
    #region Interfaces
    public interface ILevelEditor
    {
        public void ClearLevel();
        public void DeleteModule(Vector2 value);
        public void CreateModule(Vector2 value);
    }
    #endregion Interfaces

    public class LevelBuilder : MonoBehaviour, ILevelEditor
    {
        #region Parameters

        [SerializeField] GameObject _modulePrefab;

        #endregion Parameters

        #region InternalData

        [SerializeField] protected List<Module> _allModulesInScene;

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
            //Debug.Log(this.name + " - " + gameObject.name + " ClearLevel()", gameObject); 
            foreach (Module module in transform.GetComponentsInChildren<Module>())
            {
                DestroyImmediate(module.gameObject);
            }
            _allModulesInScene.Clear();
        }

        public void DeleteModule(Vector2 value) 
        {
            //Debug.Log(this.name + " - " + gameObject.name + " DeleteModule( " + value.ToString() + " )");
            rayFromSceneCamera = HandleUtility.GUIPointToWorldRay(value); //Camera.main.ScreenPointToRay(value)
            Debug.DrawRay(rayFromSceneCamera.origin, rayFromSceneCamera.direction * 10000f, Color.red, 5f);
            if (Physics.Raycast(rayFromSceneCamera, out raycastHit, 10000f))
            {
                if (raycastHit.collider.gameObject.layer != 6)
                {
                    moduleInstance = raycastHit.collider.transform.parent.parent.gameObject;
                    _allModulesInScene.Remove(moduleInstance.GetComponent<Module>());
                    DestroyImmediate(moduleInstance);
                }
            }
        }

        public void CreateModule(Vector2 value) 
        {
            //Debug.Log(this.name + " - " + gameObject.name + " CreateModule( " + value.ToString() + " )");
            rayFromSceneCamera = HandleUtility.GUIPointToWorldRay(value); //Camera.main.ScreenPointToRay(value)
            Debug.DrawRay(rayFromSceneCamera.origin, rayFromSceneCamera.direction * 10000f, Color.green, 5f);
            if (Physics.Raycast(rayFromSceneCamera, out raycastHit, 10000f)) 
            {
                if (raycastHit.collider.gameObject.layer == 6) //Layer -> Layout
                {
                    moduleInstance = Instantiate(_modulePrefab);
                    moduleInstance.transform.parent = transform;
                    modulePosition = raycastHit.point;
                    modulePosition.x = (int)modulePosition.x - 1.0f;
                    modulePosition.y = (int)modulePosition.y;
                    modulePosition.z = (int)modulePosition.z;
                    moduleInstance.transform.position = modulePosition;

                    _allModulesInScene.Add(moduleInstance.GetComponent<Module>());
                }
            }
            
        }

        #endregion InterfaceMethods
    }
}