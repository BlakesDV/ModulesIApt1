using UnityEngine;

namespace ProceduralLevelDesign
{
    public class Module : MonoBehaviour
    {
        [SerializeField] private Vector3 gridPos;

        public GameObject fullModulePrefab;
        public GameObject floorPrefab;

        public Transform floorPoint;
        public GameObject wallLeft, wallRight, wallUp, wallDown, pillarSL, pillarSR, pillarNL, pillarNR;

        private GameObject floor;

        [SerializeField] public LevelBuilder levelBuilder;
        public bool IsActive { get; private set; } = true;

        public void SetActive(bool value)
        {
            IsActive = value;
            gameObject.SetActive(value);
        }

        public void SetPosition(Vector3 pos)
        {
            gridPos = pos;
            UpdateModules();
        }

        public void UpdateModules()
        {
            UpdateWalls();
            UpdatePillars();
        }

        public void UpdateWalls()
        {
            //�module = true? N,S,W,E
            //floor = Instantiate(fullModulePrefab, floorPoint.position, Quaternion.identity, transform);
            
            bool left = !levelBuilder.ModuleSides((int)gridPos.x + 1, (int)gridPos.z); //West
            bool right = !levelBuilder.ModuleSides((int)gridPos.x - 1, (int)gridPos.z); //East
            bool up = !levelBuilder.ModuleSides((int)gridPos.x, (int)gridPos.z - 1); //North
            bool down = !levelBuilder.ModuleSides((int)gridPos.x, (int)gridPos.z + 1); //South

            //Change the module depending on the side if its colliding or not
            //Check to know if there should be a corner, wall or floor depending on each side
            //Deactivate walls if theres a neighbour
            wallLeft.SetActive(left);
            wallDown.SetActive(down);
            wallRight.SetActive(right);
            wallUp.SetActive(up);
        }
        
        public void UpdatePillars()
        {
            //�module x-+1, x+-1 y-+1 (diagonal), y-+1 = true?
            //If 3 neighbours pillar is off, if neighbours < 3 then pillar is on
            bool left = levelBuilder.ModuleSides((int)gridPos.x - 1, (int)gridPos.z); //West
            bool right = levelBuilder.ModuleSides((int)gridPos.x + 1, (int)gridPos.z); //East
            bool up = levelBuilder.ModuleSides((int)gridPos.x, (int)gridPos.z + 1); //North
            bool down = levelBuilder.ModuleSides((int)gridPos.x, (int)gridPos.z - 1); //South
            bool leftup = levelBuilder.ModuleSides((int)gridPos.x - 1, (int)gridPos.z +1);//NW
            bool leftdown = levelBuilder.ModuleSides((int)gridPos.x - 1, (int)gridPos.z - 1);//SW
            bool rightup = levelBuilder.ModuleSides((int)gridPos.x + 1, (int)gridPos.z + 1);//NE
            bool rightdown = levelBuilder.ModuleSides((int)gridPos.x + 1, (int)gridPos.z - 1);//SE

            pillarNL.SetActive(!(down && right && rightdown));
            pillarNR.SetActive(!(down && left && leftdown));
            pillarSL.SetActive(!(up && right && rightup));
            pillarSR.SetActive(!(up && left && leftup));
        }

        public LevelBuilder SetLevelBuilder
        {
            set { levelBuilder = value; }
        }

        public Vector3 GridPos
        {
            get { return gridPos; }
            set { gridPos = value; }
        }        
    }
}
