using UnityEngine;

public enum GameState
{
    Idle,
    InProgress,
    Ended
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState State { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
