using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameOfLife : MonoBehaviour
{
    public int width = 20;
    public int height = 20;
    //public float updateInterval = 0.5f;
    public GameObject cellPrefab;

    private bool[,] grid;  //stores whether cell is dead or alive
    private GameObject[,] cellObjects;  //reference to the cell gameObjects
    //private float timer;

    void Start()
    {
        //Initializes the grid and object arrays
        grid = new bool[width, height];
        cellObjects = new GameObject[width, height];

        //visual grid
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //spawn a cell prefab at the (x, y) position
                GameObject obj = Instantiate(cellPrefab, new Vector2(x, y), Quaternion.identity);
                cellObjects[x, y] = obj;

                //randomly set some cells to alive
                grid[x, y] = Random.value > 0.7f;

                //Update the visual appearance
                UpdateCellVisual(x, y);
            }
        }
    }

    void Update()
    {
        //go forward a turn when space is hit
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Step();           
        }       
    }

    /// <summary>
    /// Calculates the next generation of the grid based on Conway's rules.
    /// </summary>
    void Step()
    {
        bool[,] newGrid = new bool[width, height];
        
        //loop through every cell to determine next state
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbors = CountNeighbors(x, y);

                if (grid[x, y])
                {
                    //Rule 1 and 2: Alive cell stays alive with 2 or 3 neighbors
                    newGrid[x, y] = (neighbors == 2 || neighbors == 3);
                }
                else
                {
                    //Rule 3: Dead cell becomes alive if it has exactly 3 neighbors
                    newGrid[x, y] = (neighbors == 3);
                }
            }
        }
        //replace old grid with new generation
        grid = newGrid;

        //Update visuals for all cells
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                UpdateCellVisual(x, y);
            }
        }
    }

    /// <summary>
    /// Counts how many alive neighbors surround a cell at (x, y)
    /// </summary>
    int CountNeighbors(int x, int y)
    {
        int count = 0;

        //check all surrounding cells (8)
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int nx = x + dx;
                int ny = y + dy;

                //make sure neighbor is within the grid bounds
                if(nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    if (grid[nx, ny]) count++;
                }
            }
        }
        return count;
    }

    /// <summary>
    /// Enables or disable the visual cell object based on alive/dead state.
    /// </summary>
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