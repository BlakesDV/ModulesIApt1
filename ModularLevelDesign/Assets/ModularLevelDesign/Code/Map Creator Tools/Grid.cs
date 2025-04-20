using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid Instance;
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
        //if ()
        //{

        //}
    }
    //Create function for checking module sides
    //Create function for checking if the module is inside the grid
}
