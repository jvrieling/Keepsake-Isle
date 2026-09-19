using System;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;

    public int Score { get; private set; }

    private void Start()
    {
        GridObject.OnAnyObjectCleared += HandleAnyObjectCleared;
    }

    private void OnDestroy()
    {
        GridObject.OnAnyObjectCleared -= HandleAnyObjectCleared;
    }

    private void RefreshUI()
    {
        scoreText.text = Score.ToString("00000");
    }

    private void HandleAnyObjectCleared(GridObject gridObject)
    {
        Score += gridObject.ScoreValue;

        RefreshUI();
    }
}
