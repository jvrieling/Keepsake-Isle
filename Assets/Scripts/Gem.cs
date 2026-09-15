using UnityEngine;

public class Gem : GridObject
{
    private void Awake()
    {
        OnGrounded += HandleGrounded;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnGrounded -= HandleGrounded;
    }

    private void HandleGrounded(GridObject gridObject)
    {

    }
}
