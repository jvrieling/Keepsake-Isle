using UnityEngine;
using System.Collections.Generic;
using ChickenCoop.Util;
using System;
using System.Collections;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class Column : IEnumerable<GridCell>
{
    [SerializeField]
    public List<GridCell> cells;
    public GridObject hangingObject;

    public Column()
    {
        cells = new List<GridCell>();
    }

    public void SetHangingObject(GridObject newObject)
    {
        if (newObject.State != GridObjectState.Hanging) return;

        ClearHangingObject();

        hangingObject = newObject;

        hangingObject.OnDestroyed += HandleDestroyed;
        hangingObject.OnFallingStarted += HandleFallingStarted;
    }

    private void ClearHangingObject()
    {
        if (hangingObject == null) return;

        hangingObject.OnDestroyed -= HandleDestroyed;
        hangingObject.OnFallingStarted -= HandleFallingStarted;
        hangingObject = null;
    }

    private void HandleFallingStarted(GridObject @object)
    {
        ClearHangingObject();
    }

    private void HandleDestroyed(GridObject @object)
    {
        ClearHangingObject();
    }

    public void Add(GridCell cell)
    {
        cells.Add(cell);
    }

    public IEnumerator<GridCell> GetEnumerator()
    {
        foreach (GridCell gridObject in cells)    
        {
            yield return gridObject;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public GridCell this[int index]
    {
        get => cells[index];
        set => cells[index] = value;
    }

    public static implicit operator List<GridCell>(Column col) => col.cells;
}

public class Grid : MonoBehaviour
{
    public const float CELL_SIZE = 19f / 100f;

    public static Grid Instance;

    [SerializeField]
    private List<Column> grid;

    [SerializeField]
    private int width;
    public int Width => width;

    [SerializeField]
    private int height;
    public int Height => height;

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

        grid = new List<Column>();

        Vector2 currentPosition = bottomLeftCell.position;

        for (int i = 0; i < width; i++)
        {
            grid.Add(new Column());

            for (int j = 0; j < height; j++)
            {
                GridCell cell = new GridCell();

                cell.InitializeCell(currentPosition, i, j);

                grid[i].Add(cell);

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

        foreach (Column col in grid)
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

    public GridCell GetAtCoords((int, int) coords)
    {
        if (coords.Item1 < 0 || coords.Item2 < 0 || coords.Item1 >= grid.Count || coords.Item2 >= grid[0].cells.Count) return null;

        return grid[coords.Item1][coords.Item2];
    }

    public Column GetRandomColumn()
    {
        if (grid == null) return null;

        return grid.GetRandomElement();
    }

    public Column GetRandomAvailableColumn()
    {
        if (grid == null) return null;

        List<Column> availableColumns = grid.Where(c => c.hangingObject == null).ToList();

        return availableColumns.GetRandomElement();
    }

    public Column GetNearestColumn(Vector2 worldPosition)
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

        if (worldPosition.y < grid[0].cells[0].WorldPosition.y - (CELL_SIZE / 2))
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

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        if (grid == null ) return;
        Handles.color = Color.green;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (grid[i] == null || grid[i].cells == null || grid[i][j] == null) return;

                if (grid[i][j].IsOccupied)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.rebeccaPurple;
                }

                Gizmos.DrawSphere(grid[i][j].WorldPosition, 0.01f);
                Handles.Label(grid[i][j].WorldPosition * 1.1f, $"({grid[i][j].GridIndex.Item1}, {grid[i][j].GridIndex.Item2})");
            }
        }
    }
#endif
}
