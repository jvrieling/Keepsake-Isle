using GBTemplate;
using System.Collections;
using TMPro;
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

    [SerializeField]
    private ScoreSystem scoreSystem;

    [SerializeField]
    private float gameEndPause = 5;

    [SerializeField]
    private float fadeSpeed = 0.75f;

    [SerializeField]
    private GameObject gameOverScreen;

    [SerializeField]
    private AudioClip gameOverClip;

    [SerializeField]
    private TMP_Text finalScoreText;

    [SerializeField]
    private AudioClip finalScoreSound;

    public GameState State { get; private set; }

    private GBConsoleController gb;

    private void Awake()
    {
        Instance = this;

        GridObject.OnAnyObjectCannotFall += HandleCannotFall;

        State = GameState.InProgress;

        gb = GBConsoleController.GetInstance();

        gameOverScreen.SetActive(false);
    }

    private void OnDestroy()
    {
        GridObject.OnAnyObjectCannotFall += HandleCannotFall;
    }

    private IEnumerator GameEndRoutine()
    {
        finalScoreText.text = "";

        yield return new WaitForSeconds(gameEndPause);

        yield return gb.Display.StartCoroutine(gb.Display.FadeToBlack(fadeSpeed));

        gameOverScreen.SetActive(true);

        yield return gb.Display.StartCoroutine(gb.Display.FadeFromBlack(fadeSpeed * 2));

        yield return new WaitForSeconds(0.5f);

        gb.Sound.PlaySound(finalScoreSound);
        finalScoreText.text = $"Final Score: " + scoreSystem.Score.ToString();
    }

    [ContextMenu("End Game")]
    private void DEBUG_EndGame()
    {
        if (gb == null)
        {
            gb = GBConsoleController.GetInstance();
        }
        State = GameState.Ended;
        gb.Sound.StopMusic();
        gb.Sound.PlaySound(gameOverClip);
        StartCoroutine(GameEndRoutine());
    }

    private void HandleCannotFall(GridObject gridObject)
    {
        if (gb == null)
        {
            gb = GBConsoleController.GetInstance();
        }
        State = GameState.Ended;
        gb.Sound.StopMusic();
        gb.Sound.PlaySound(gameOverClip);
        StartCoroutine(GameEndRoutine());
    }
}
