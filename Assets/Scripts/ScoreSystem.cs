using System;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private TMP_Text comboText;

    [SerializeField]
    private float comboDecayTime = 3;

    public int Score { get; private set; }
    public int Combo {  get; private set; }

    private float comboDecayTimer;

    private void Start()
    {
        GridObject.OnAnyObjectCleared += HandleAnyObjectCleared;
        GridObject.OnAnyScoringMatchMade += HandleScoringMatch;
    }

    private void OnDestroy()
    {
        GridObject.OnAnyObjectCleared -= HandleAnyObjectCleared;
        GridObject.OnAnyScoringMatchMade -= HandleScoringMatch;
    }

    private void Update()
    {
        if (comboDecayTimer > 0)
        {
            comboDecayTimer -= Time.deltaTime;

            if (comboDecayTimer <= 0)
            {
                Combo = 0;
            }
        }
    }

    private void RefreshUI()
    {
        scoreText.text = Score.ToString("00000");
        comboText.text = Combo.ToString();
    }

    private void HandleScoringMatch(GridObject gridObject)
    {
        Combo++;
        comboDecayTimer = comboDecayTime;

        RefreshUI();
    }

    private void HandleAnyObjectCleared(GridObject gridObject)
    {
        Score += gridObject.ScoreValue * Combo;

        RefreshUI();
    }
}
