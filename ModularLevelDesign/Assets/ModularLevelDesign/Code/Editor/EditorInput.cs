using UnityEngine;
using UnityEditor;

namespace ProceduralLevelDesign {
    public class EditorInput
    {
        #region LocalMethods

        #region UnityMethods

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void ScriptsHasBeenReloaded()
        {
            SceneView.duringSceneGui += DuringSceneGui;
        }
        #endregion UnityMethods

        #region DelegateMethods

        private static void DuringSceneGui (SceneView sceneView) //OnDrawGizmos()
        {
            Event e = Event.current; //equivalent to InputAction.CallbackContext / InputValue
            //Event stores data input from the level designer / programmer
            //Debug.Log("EditorInput - DuringSceneGui(): " + e);
            LevelBuilder levelBuilder = GameObject.FindFirstObjectByType<LevelBuilder>();

            if (e.type == EventType.KeyUp && e.keyCode == KeyCode.Delete) //"Supr / Suprimir"
            {
                //TODO: Method to clean all the level.
                levelBuilder?.ClearLevel();
            } 
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Backspace)//Delete / "Borrar"
            {
                //TODO: Methos to delet a tile / module from the scene.
                levelBuilder?.DeleteModule(e.mousePosition);
            }
            if (e.type == EventType.MouseUp && e.button == 0)
            {
                //TODO: Method to instantiate a tile / module in the scene.
                levelBuilder?.CreateModule(e.mousePosition);
            }
        }
        #endregion DelegateMethods

        #endregion Local Methods
    }
}