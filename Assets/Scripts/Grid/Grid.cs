using UnityEngine;
using System.Collections.Generic;
using ChickenCoop.Util;

public class Grid : MonoBehaviour
{
    public const float CELL_SIZE = 19f / 100f;

    public static Grid Instance;

    [SerializeField]
    private List<List<GridCell>> grid;

    [SerializeField]
    private int width;

    [SerializeField]
    private int height;

    [SerializeField]
    private Transform bottomLeftCell;

    public int totalCells;

    public float worldCellSize;

    private void OnValidate()
    {
        totalCells = 0;
        if (grid != null)
        foreach (var col in grid)
        {
            foreach (var cell in col)
            {
                totalCells++;
            }
        }

        worldCellSize = CELL_SIZE;
    }

    private void Awake()
    {
        Instance = this;

        worldCellSize = CELL_SIZE;
        GenerateGrid();
    }

    [ContextMenu("Generate Grid")]
    public void GenerateGrid()
    {
        DestroyGrid();

        grid = new List<List<GridCell>>();

        Vector2 currentPosition = bottomLeftCell.position;

        for (int i = 0; i < width; i++)
        {
            grid.Add(new List<GridCell>());

            for (int j = 0; j < height; j++)
            {
                GridCell cell = new GridCell();

                cell.InitializeCell(currentPosition, i, j);

                grid[i].Add(cell);

                //Debug.Log($"Generated cell at {currentPosition} in {i}{j}");

                currentPosition.y += worldCellSize;
            }

            currentPosition.y = bottomLeftCell.position.y;
            currentPosition.x += worldCellSize;
        }
    }

    [ContextMenu("Destroy Grid")]
    public void DestroyGrid()
    {
        if (grid == null) return;

        foreach (List<GridCell> col in grid)
        {
            if (col == null) continue;

            foreach(GridCell cell in col)
            {
                if (cell == null) continue;

                cell.DestroyObject();
            }
        }

        grid.Clear();
    }

    public List<GridCell> GetRandomColumn()
    {
        if (grid == null) return null;

        return grid.GetRandomElement();
    }

    public List<GridCell> GetNearestColumn(Vector2 worldPosition)
    {
        float nearestDistance = float.MaxValue;
        int nearestColIndex = -1;

        for (int i = 0; i < grid.Count; i++)
        {
            List<GridCell> col = grid[i];
            float dist = Vector2.Distance(worldPosition, col[0].WorldPosition);
            if (dist < nearestDistance)
            {
                nearestDistance = dist;
                nearestColIndex = i;
            }
        }

        if (nearestColIndex == -1) return null;

        return grid[nearestColIndex];
    }

    public GridCell GetNearestCell(Vector2 worldPosition)
    {
        float nearestDistance = float.MaxValue;
        GridCell nearestCell = null;

        if (worldPosition.y < grid[0][0].WorldPosition.y - (CELL_SIZE / 2))
        {
            return null;
        }

        foreach (List<GridCell> column in grid)
        {
            foreach (GridCell cell in column)
            {
                float dist = Vector2.Distance(worldPosition, cell.WorldPosition);
                if (dist < nearestDistance && dist < CELL_SIZE)
                {
                    nearestDistance = dist;
                    nearestCell = cell;
                }
            }
        }

        return nearestCell;
    }

    public void OnDrawGizmosSelected()
    {
        if (grid == null ) return; 
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (grid[i] == null || grid[i][j] == null) return;

                if (grid[i][j].IsOccupied)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.rebeccaPurple;
                }

                Gizmos.DrawSphere(grid[i][j].WorldPosition, 0.01f);
            }
        }
    }
}
