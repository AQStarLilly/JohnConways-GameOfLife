using Unity.VisualScripting;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public int width = 20;
    public int height = 20;
    public float updateInterval = 0.5f;
    public GameObject cellPrefab;


}


if cell < 2 live neigbours, cell dies
if cell == 2 live neighbours || 3 live neighbours, cell lives
if cell > 3 live neighbours, cell dies
if cell = Dead cell && cell == 3 live neighbours, cell spawns

//Any live cell with fewer than two live neighbours dies, as if by underpopulation.
//Any live cell with two or three live neighbours lives on to the next generation.
//Any live cell with more than three live neighbours dies, as if by overpopulation.
//Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
//Cells occupy a 2D grid 