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

    public void SetGridObject(GridObject newObject)
    {
        if (GridObject != null)
        {
            GridObject.OnDestroyed -= HandleObjectDestroyed;
        }

        GridObject = newObject;

        if (GridObject != null)
        {
            GridObject.OnDestroyed += HandleObjectDestroyed;

            GridObject.transform.DOKill(true);
            GridObject.transform.DOMove(WorldPosition, tweenDuration);
        }
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
