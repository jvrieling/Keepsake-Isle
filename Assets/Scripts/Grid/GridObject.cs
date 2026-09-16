using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridObjectState
{
    None,
    Hanging,
    Falling,
    Grounded
}

public abstract class GridObject : MonoBehaviour
{
    private const float HANG_Y = 0.655f;

    public event Action<GridObject> OnDestroyed;
    public event Action<GridObject> OnGrounded;
    public event Action<GridObject> OnFallingStarted;

    [SerializeField]
    private float hangTime = 1;

    [SerializeField]
    private float fallSpeed = 0.2f;

    [SerializeField]
    protected int colourId = 1;

    [SerializeField]
    protected float timeBetweenClears = 0.2f;

    [SerializeField]
    private ParticleSystem clearParticle;

    public GridObjectState State { get; private set; } = GridObjectState.Hanging;
    public bool IsFalling { get; private set; } = true;
    public bool IsGrounded => !IsFalling;
    public bool MarkedForClearing { get; private set; }

    private Coroutine clearRoutine;
    public GridCell currentCell;
    public Vector2 lastCheckedPos;

    private bool hasBeenGroundedBefore;
    private float hangDuration;
    private float distanceTravelledSinceLastCheck;

    protected virtual void Start()
    {
        Column column = Grid.Instance.GetRandomAvailableColumn();
        column.SetHangingObject(this);

        Vector3 startingPos = Vector3.zero;
        startingPos.x = column[0].WorldPosition.x;
        startingPos.y = HANG_Y;

        transform.position = startingPos;
    }

    protected virtual void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }

    protected virtual void Update()
    {
        if (State == GridObjectState.Hanging)
        {
            hangDuration += Time.deltaTime;

            if (hangDuration > hangTime) 
            {
                if (CheckForSpaceBelow())
                {
                    State = GridObjectState.Falling;
                    OnFallingStarted?.Invoke(this);
                }
                else
                {
                    Debug.Log("!! -- NO SPACE TO FALL -- !!");
                }
            }
        }
        else if (State == GridObjectState.Falling)
        {
            float fallDist = fallSpeed * Time.deltaTime;
            transform.position += Vector3.down * fallDist;

            distanceTravelledSinceLastCheck += fallDist;

            if (distanceTravelledSinceLastCheck > Grid.CELL_SIZE)
            {
                distanceTravelledSinceLastCheck = 0;
                
                if (!CheckForSpaceBelow())
                {
                    State = GridObjectState.Grounded;
                    GridCell occupiedCell = Grid.Instance.GetNearestCell(transform.position);
                    transform.position = occupiedCell.WorldPosition;
                    occupiedCell.SetGridObject(this);

                    if (!hasBeenGroundedBefore)
                    {
                        TryClear();
                        hasBeenGroundedBefore = true;
                    }

                    OnGrounded?.Invoke(this);
                }
            }
        }
    }

    protected virtual void TryClear()
    {
        List<GridCell> column = Grid.Instance.GetNearestColumn(transform.position);

        if (column == null)
        {
            return;
        }

        bool connectionMade = false;
        int thisIndex = -1;
        int matchindIndex = -1;
        int blockerIndex = -1;

        // Start at the top of the column and go down
        for (int i = column.Count - 1; i >= 0; i--)
        {
            if (column[i] == null || column[i].GridObject == null)
            {
                continue;
            }

            // If there's clearing objects already in this column, don't allow any clearing.
            if (column[i].GridObject.MarkedForClearing) return;

            // Mark the blocker index if we find one
            if (colourId != -1 && column[i].GridObject.colourId == -1)
            {
                blockerIndex = i;
            }

            if (column[i].GridObject == this)
            {
                thisIndex = i;
                continue;
            }

            if (blockerIndex == -1 && column[i].GridObject.colourId == this.colourId)
            {
                connectionMade = true;
                matchindIndex = i;
            }
        }

        if (connectionMade)
        {
            column[matchindIndex].GridObject.TriggerClearRoutine(column.GetRange(matchindIndex, thisIndex - matchindIndex + 1));
        }
    }

    protected void OnCleared()
    {
        Instantiate(clearParticle, transform.position, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }

    private void MarkForClear()
    {
        MarkedForClearing = true;
        transform.localScale = transform.localScale * 0.8f;
    }

    private void TriggerClearRoutine(List<GridCell> cells)
    {
        foreach (GridCell cell in cells)
        {
            cell.GridObject.MarkForClear();
        }

        if (clearRoutine != null) throw new Exception("Cannot run two clear routines on one GridObject!!");
        clearRoutine = StartCoroutine(ClearCells(cells));
    }

    private IEnumerator ClearCells(List<GridCell> cells)
    {
        yield return new WaitForSeconds(timeBetweenClears);

        for (int i = cells.Count - 1; i >= 0; i--)
        {
            GridCell cell = cells[i];

            cell.GridObject.OnCleared();

            yield return new WaitForSeconds(timeBetweenClears);
        }
    }

    /// <returns>True if the cell below is open. False if occupied or off grid</returns>
    public bool CheckForSpaceBelow()
    {
        lastCheckedPos = transform.position + Vector3.down * Grid.CELL_SIZE;

        GridCell nextCell = Grid.Instance.GetNearestCell(transform.position + Vector3.down * Grid.CELL_SIZE);

        if (nextCell == null || nextCell.IsOccupied)
        {
            return false;
        }

        return true;
    }

    private void OnDrawGizmos()
    {
        if (lastCheckedPos == Vector2.zero) return;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(lastCheckedPos, 0.02f);
    }
}
