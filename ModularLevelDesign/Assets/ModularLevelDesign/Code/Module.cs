using UnityEngine;

namespace ProceduralLevelDesign
{
    public class Module : MonoBehaviour
    {
        private Vector2Int gridPos;

        public GameObject wallPrefab;
        public GameObject floorPrefab;

        public Transform wallLeftPoint;
        public Transform wallRightPoint;
        public Transform wallUpPoint;
        public Transform wallDownPoint;
        public Transform floorPoint;

        private GameObject wallLeft, wallRight, wallUp, wallDown, floor;


        public void SetPosition(Vector2Int pos)
        {
            gridPos = pos;
            UpdateRender();

        }

        private void OnDrawGizmos()
        {
            UpdateRender();
        }

        public void UpdateRender()
        {
            floor = Instantiate(floorPrefab, floorPoint.position, Quaternion.identity, transform);
            
            bool left = LevelBuilder.ModuleSides(gridPos.x - 1, gridPos.y);
            bool right = LevelBuilder.Instance.ModuleSides(gridPos.x + 1, gridPos.y);
            bool up = LevelBuilder.Instance.ModuleSides(gridPos.x, gridPos.y + 1);
            bool down = LevelBuilder.Instance.ModuleSides(gridPos.x, gridPos.y - 1);

            //Renderer renderer = GetComponent<Renderer>();

            //Change the module depending on the side if its colliding or not
            //Check to know if there shoul be a corner, wall or floor depending on each side
            //Deactivate walls if theres a neighbour
            //If 3 neighbours pillar is off, if neighbours < 3 then pillar is on
        }
    }
}
