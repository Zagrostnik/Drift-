using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    [Header("Score Settings")][SerializeField] private int lapBonus = 1000; [SerializeField] private int collisionPenalty = 500; [SerializeField] private float driftScoreInterval = 0.25f;
    [Header("UI Elements")]
    [SerializeField] private TMP_Text scoreText;     // Перетащите сюда текст общего счета из Canvas
    [SerializeField] private TMP_Text popupText;    // (Опционально) Текст для всплывающих очков, например "+5"

    public int CurrentScore { get; private set; }
    private float driftTimer;

    private void Start()
    {
        // Очищаем всплывающий текст при старте
        if (popupText != null) popupText.text = "";
        UpdateScoreUI(CurrentScore);
    }

    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        if (GameManager.Instance.State != GameState.Playing)
            return;

        driftTimer += Time.deltaTime;

        if (driftTimer >= driftScoreInterval)
        {
            TryAwardDriftScore();
            driftTimer = 0f;
        }
    }

    private void TryAwardDriftScore()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;
        DriftDetector detector = player.GetComponent<DriftDetector>();
        if (detector == null || !detector.IsDrifting)
        {
            if (popupText != null && popupText.text.StartsWith("+"))
            {
                popupText.text = "";
            }
            return;
        }

        DriftScoreStrategy strategy = new DriftScoreStrategy(detector.DriftAngle);
        int earnedScore = strategy.CalculateScore();
        if (earnedScore > 0)
        {

            AddScore(earnedScore);
            if (popupText != null)
            {
                popupText.text = "+" + earnedScore;
            }
        }

        // Показываем всплывающий текст, если он задан
        ShowPopup("+" + earnedScore);
    }

    public void AwardLapBonus()
    {
        ApplyStrategy(new LapScoreStrategy(lapBonus));
        ShowPopup("КРУГ ПРОЙДЕН! +" + lapBonus);
    }

    public void ApplyCollisionPenalty()
    {
        ApplyStrategy(new CollisionPenaltyStrategy(collisionPenalty));
        ShowPopup("-" + collisionPenalty);
    }

    private void ApplyStrategy(ScoringStrategy strategy)
    {
        AddScore(strategy.CalculateScore());
    }

    private void AddScore(int amount)
    {
        CurrentScore = Mathf.Max(0, CurrentScore + amount);

        // Проверяем, существует ли класс событий, чтобы не было ошибок компиляции
        // GameEvents.RaiseScoreChanged(CurrentScore); 

        UpdateScoreUI(CurrentScore);
        Debug.Log("Current score: " + CurrentScore);
    }

    private void UpdateScoreUI(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Очки: " + score;
        }
    }

    private void ShowPopup(string message)
    {
        if (popupText != null)
        {
            popupText.text = message;
            // Здесь можно добавить простенькое затухание или просто выводить текст
        }
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        driftTimer = 0f;
        UpdateScoreUI(CurrentScore);
        if (popupText != null) popupText.text = "";
    }
}
