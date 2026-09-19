using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class GridCell
{
    [field: SerializeField]
    public GridObject GridObject { get; private set; }
    public (int, int) GridIndex { get; private set; }
    [field: SerializeField]
    public Vector2 WorldPosition { get; private set; }
    [SerializeField]
    private float tweenDuration = 0.1f;

    public bool IsOccupied => GridObject != null;

    public void InitializeCell(Vector2 worldPos, int column, int row)
    {
        WorldPosition = worldPos;
        GridIndex = (column, row);
    }

    public void SetGridObject(GridObject newObject, bool allowDestroyOld = false)
    {
        if (GridObject != null)
        {
            GridObject.OnDestroyed -= HandleObjectDestroyed;
            GridObject.OnFallingStarted -= HandleObjectFall;

            if (allowDestroyOld) DestroyObject();
        }

        GridObject = newObject;

        if (GridObject != null)
        {
            GridObject.SetCurrentCell(this);

            GridObject.OnDestroyed += HandleObjectDestroyed;
            GridObject.OnFallingStarted += HandleObjectFall;

            GridObject.transform.DOKill(true);
            GridObject.transform.DOMove(WorldPosition, tweenDuration).OnComplete(GridObject.CheckUngrounded);
        }
        else
        {
            CheckCellAboveGrounded();
        }
    }

    public bool DestroyObject()
    {
        if (GridObject == null) return false;

        GameObject.Destroy(GridObject.gameObject);

        return true;
    }

    private void CheckCellAboveGrounded()
    {
        GridCell cellAbove = Grid.Instance.GetAtCoords((GridIndex.Item1, GridIndex.Item2 + 1));

        if (cellAbove != null && cellAbove.GridObject != null)
        {
            cellAbove.GridObject.CheckUngrounded();
        }
    }

    private void HandleObjectDestroyed(GridObject gridObject)
    {
        if (gridObject == GridObject)
        {
            GridObject.OnFallingStarted -= HandleObjectFall;
            GridObject.OnDestroyed -= HandleObjectDestroyed;
            GridObject = null;

            CheckCellAboveGrounded();
        }
    }

    private void HandleObjectFall(GridObject gridObject)
    {
        if (gridObject == GridObject)
        {
            GridObject.OnFallingStarted -= HandleObjectFall;
            GridObject.OnDestroyed -= HandleObjectDestroyed;
            GridObject = null;

            CheckCellAboveGrounded();
        }
    }
}
