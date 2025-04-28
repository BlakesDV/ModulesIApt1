using UnityEngine;

namespace ProceduralLevelDesign
{
    public class Module : MonoBehaviour
    {
        private Vector2Int gridPos;

        public GameObject fullModulePrefab;
        public GameObject floorPrefab;

        public Transform floorPoint;
        public GameObject wallLeft, wallRight, wallUp, wallDown, pillarSL, pillarSR, pillarNL, pillarNR;

        private GameObject floor;

        private LevelBuilder levelBuilder;


        public void SetPosition(Vector2Int pos)
        {
            gridPos = pos;
            UpdateWalls();

        }

        private void OnDrawGizmos()
        {
            //UpdateWalls();
        }
        public void UpdateModules()
        {
            UpdateWalls();
            UpdatePillars();
        }

        public void UpdateWalls()
        {
            //¿module = true? N,S,W,E
            floor = Instantiate(fullModulePrefab, floorPoint.position, Quaternion.identity, transform);
            
            bool left = levelBuilder.ModuleSides(gridPos.x - 1, gridPos.y); //West
            bool right = levelBuilder.ModuleSides(gridPos.x + 1, gridPos.y); //East
            bool up = levelBuilder.ModuleSides(gridPos.x, gridPos.y + 1); //North
            bool down = levelBuilder.ModuleSides(gridPos.x, gridPos.y - 1); //South

            //Change the module depending on the side if its colliding or not
            //Check to know if there shoul be a corner, wall or floor depending on each side
            //Deactivate walls if theres a neighbour
            //If 3 neighbours pillar is off, if neighbours < 3 then pillar is on
        }
        
        public void UpdatePillars()
        {
            //¿module x-+1, x+-1 y-+1 (diagonal), y-+1 = true?
            bool left = levelBuilder.ModuleSides(gridPos.x - 1, gridPos.y); //West
            bool right = levelBuilder.ModuleSides(gridPos.x + 1, gridPos.y); //East
            bool up = levelBuilder.ModuleSides(gridPos.x, gridPos.y + 1); //North
            bool down = levelBuilder.ModuleSides(gridPos.x, gridPos.y - 1); //South
            bool northwest = levelBuilder.ModuleSides(gridPos.x - 1, gridPos.y +1);//NW
            bool southwest = levelBuilder.ModuleSides(gridPos.x - 1, gridPos.y - 1);//SW
            bool northeast = levelBuilder.ModuleSides(gridPos.x + 1, gridPos.y + 1);//NE
            bool southeast = levelBuilder.ModuleSides(gridPos.x + 1, gridPos.y - 1);//SE

        }
    }
}
