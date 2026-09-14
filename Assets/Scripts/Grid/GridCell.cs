using System;
using UnityEngine;

[Serializable]
public class GridCell
{
    public GridObject GridObject { get; private set; }
    public (int, int) GridIndex { get; private set; }
    public Vector2 WorldPosition { get; private set; }

    public bool IsOccupied => GridObject != null;

    public void InitializeCell(Vector2 worldPos, int column, int row)
    {
        WorldPosition = worldPos;
        GridIndex = (column, row);
    }

    public void SetGridObject(GridObject newObject)
    {
        if (GridObject != null)
        {
            GridObject.OnDestroyed -= HandleObjectDestroyed;
        }

        GridObject = newObject;
        GridObject.OnDestroyed += HandleObjectDestroyed;
    }

    public bool DestroyObject()
    {
        if (GridObject == null) return false;

        GameObject.Destroy(GridObject.gameObject);

        return true;
    }

    private void HandleObjectDestroyed(GridObject gridObject)
    {
        if (gridObject == GridObject)
        {
            GridObject.OnDestroyed -= HandleObjectDestroyed;
            GridObject = null;
        }
    }
}
