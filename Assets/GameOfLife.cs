using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameOfLife : MonoBehaviour
{
    public int width = 20;
    public int height = 20;
    //public float updateInterval = 0.5f;
    public GameObject cellPrefab;

    private bool[,] grid;
    private GameObject[,] cellObjects;
    //private float timer;

    void Start()
    {
        grid = new bool[width, height];
        cellObjects = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject obj = Instantiate(cellPrefab, new Vector2(x, y), Quaternion.identity);
                cellObjects[x, y] = obj;

                grid[x, y] = Random.value > 0.7f;
                UpdateCellVisual(x, y);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Step();           
        }       
    }

    void Step()
    {
        bool[,] newGrid = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbors = CountNeighbors(x, y);

                if (grid[x, y])
                {
                    newGrid[x, y] = (neighbors == 2 || neighbors == 3);
                }
                else
                {
                    newGrid[x, y] = (neighbors == 3);
                }
            }
        }
        grid = newGrid;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; x < height; y++)
            {
                UpdateCellVisual(x, y);
            }
        }
    }

    int CountNeighbors(int x, int y)
    {
        int count = 0;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; y++)
            {
                if (dx == 0 && dy == 0) continue;

                int nx = x + dx;
                int ny = y + dy;

                if(nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    if (grid[nx, ny]) count++;
                }
            }
        }
        return count;
    }

    void UpdateCellVisual(int x, int y)
    {
        cellObjects[x, y].SetActive(grid[x, y]);
    }

}


//if cell < 2 live neigbours, cell dies
//if cell == 2 live neighbours || 3 live neighbours, cell lives
//if cell > 3 live neighbours, cell dies
//if cell = Dead cell && cell == 3 live neighbours, cell spawns

//Any live cell with fewer than two live neighbours dies, as if by underpopulation.
//Any live cell with two or three live neighbours lives on to the next generation.
//Any live cell with more than three live neighbours dies, as if by overpopulation.
//Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
//Cells occupy a 2D grid 