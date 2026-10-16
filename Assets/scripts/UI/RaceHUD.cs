
using TMPro;
using UnityEngine;

public class RaceHUD : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text lapText;

    [Header("Game Systems")]
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private LapManager lapManager;

    private void Start()
    {
        if (scoreManager == null)
        {
            scoreManager = FindFirstObjectByType<ScoreManager>();
        }

        if (lapManager == null)
        {
            lapManager = FindFirstObjectByType<LapManager>();
        }

        UpdateScore(0);
        UpdateLap();
    }

    private void OnEnable()
    {
        GameEvents.ScoreChanged += UpdateScore;
    }

    private void OnDisable()
    {
        GameEvents.ScoreChanged -= UpdateScore;
    }

    private void Update()
    {
        UpdateLap();
    }

    private void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "СЧЁТ: " + score;
        }
    }

    private void UpdateLap()
    {
        if (lapText == null || lapManager == null)
        {
            return;
        }

        lapText.text =
            "КРУГ: " + lapManager.CompletedLaps + " / 3";
    }
}
