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

        public void UpdateRender()
        {
            floor = Instantiate(floorPrefab, floorPoint.position, Quaternion.identity, transform);
            
            bool left = GridTool.Instance.ModuleSides(gridPos.x - 1, gridPos.y);
            bool right = GridTool.Instance.ModuleSides(gridPos.x + 1, gridPos.y);
            bool up = GridTool.Instance.ModuleSides(gridPos.x, gridPos.y + 1);
            bool down = GridTool.Instance.ModuleSides(gridPos.x, gridPos.y - 1);

            //Renderer renderer = GetComponent<Renderer>();

            //Change the module depending on the side if its colliding or not
            //Check to know if there shoul be a corner, wall or floor depending on each side
            if (!left)
                wallLeft = Instantiate(wallPrefab, wallLeftPoint.position, wallLeftPoint.rotation, transform);
            if (!right)
                wallLeft = Instantiate(wallPrefab, wallRightPoint.position, wallRightPoint.rotation, transform);
            if (!up)
                wallLeft = Instantiate(wallPrefab, wallUpPoint.position, wallUpPoint.rotation, transform);
            if (!down)
                wallLeft = Instantiate(wallPrefab, wallDownPoint.position, wallDownPoint.rotation, transform);
        }
    }
}
