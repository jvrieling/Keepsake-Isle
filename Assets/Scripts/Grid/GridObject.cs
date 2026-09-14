using System;
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
    private const float HANG_Y = 0.646f;

    public event Action<GridObject> OnDestroyed;

    [SerializeField]
    private float hangTime = 1;

    [SerializeField]
    private float fallSpeed = 0.2f;

    public GridObjectState State { get; private set; } = GridObjectState.Hanging;
    public bool IsFalling { get; private set; } = true;
    public bool IsGrounded => !IsFalling;

    public GridCell currentCell;

    private float hangDuration;
    private float distanceTravelledSinceLastCheck;

    private void Start()
    {
        Vector3 startingPos = Vector3.zero;

        startingPos.y = HANG_Y;

        startingPos.x = Grid.Instance.GetRandomColumn()[0].WorldPosition.x;

        transform.position = startingPos;
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }

    private void Update()
    {
        if (State == GridObjectState.Hanging)
        {
            hangDuration += Time.deltaTime;

            if (hangDuration > hangTime) 
            {
                State = GridObjectState.Falling;
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

                GridCell nextCell = Grid.Instance.GetNearestCell(transform.position + Vector3.down * Grid.CELL_SIZE);

                if (nextCell == null || nextCell.IsOccupied)
                {
                    if (nextCell == null)
                    {
                        Debug.Log("Landed on the ground!!");
                    }

                    State = GridObjectState.Grounded;

                    GridCell occupiedCell = Grid.Instance.GetNearestCell(transform.position);
                    transform.position = occupiedCell.WorldPosition;
                    occupiedCell.SetGridObject(this);
                }
            }
        }
    }
}
