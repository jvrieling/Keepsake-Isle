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
        scoreText.text = Score.ToString();
    }

    private void HandleAnyObjectCleared(GridObject @object)
    {
        Score += @object.ScoreValue;

        RefreshUI();
    }
}
