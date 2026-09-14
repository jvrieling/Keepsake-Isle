using System;
using UnityEngine;

public enum GridObjectState
{
    None,
    Hanging,
    Falling,
    Grounded
}

public class GridObject : MonoBehaviour
{
    public event Action<GridObject> OnDestroyed;

    [SerializeField]
    private float hangTime = 1;

    [SerializeField]
    private float fallSpeed = 0.2f;

    public GridObjectState State { get; private set; } = GridObjectState.Hanging;
    public bool IsFalling { get; private set; } = true;
    public bool IsGrounded => !IsFalling;

    public GridCell currentCell;

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }

    private void Update()
    {
        

    }
}
