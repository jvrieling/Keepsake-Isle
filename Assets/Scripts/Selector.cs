using DG.Tweening;
using GBTemplate;
using UnityEngine;

public class Selectors : MonoBehaviour
{
    [SerializeField]
    private float tweenDuration = 0.5f;

    public GridCell mainCell, rightCell;

    private GBConsoleController gb;
    private GBInputController Input => gb.Input;

    private void Start()
    {
        gb = GBConsoleController.GetInstance();

        mainCell = Grid.Instance.GetAtCoords((
            Mathf.RoundToInt(Grid.Instance.Width / 2),
            Mathf.RoundToInt(Grid.Instance.Height / 2)));

        rightCell = Grid.Instance.GetAtCoords((
            mainCell.GridIndex.Item1 + 1,
            mainCell.GridIndex.Item2));

        TryMove(mainCell);
    }

    private void Update()
    {
        if (Input.LeftJustPressed)
        {
            TryMove(Grid.Instance.GetAtCoords((
            mainCell.GridIndex.Item1 - 1,
            mainCell.GridIndex.Item2)));
        }
        else if (Input.RightJustPressed)
        {
            TryMove(Grid.Instance.GetAtCoords((
            mainCell.GridIndex.Item1 + 1,
            mainCell.GridIndex.Item2)));
        }
        else if (Input.UpJustPressed)
        {
            TryMove(Grid.Instance.GetAtCoords((
            mainCell.GridIndex.Item1,
            mainCell.GridIndex.Item2 + 1)));
        }
        else if (Input.DownJustPressed)
        {
            TryMove(Grid.Instance.GetAtCoords((
            mainCell.GridIndex.Item1,
            mainCell.GridIndex.Item2 - 1)));
        }
    }

    private void TryMove(GridCell cell)
    {
        if (cell == null) return;
        GridCell cellToTheRight = Grid.Instance.GetAtCoords((cell.GridIndex.Item1 + 1, cell.GridIndex.Item2));

        if (cellToTheRight == null) return;

        Vector3 newPosition = new Vector3(
            (cell.WorldPosition.x + cellToTheRight.WorldPosition.x) / 2,
            (cell.WorldPosition.y + cellToTheRight.WorldPosition.y) / 2);

        transform.DOMove(newPosition, tweenDuration);
        mainCell = cell;
        rightCell = cellToTheRight;
    }
}
