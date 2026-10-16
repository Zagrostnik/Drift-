
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Score Settings")]
    [SerializeField] private int lapBonus = 1000;
    [SerializeField] private int collisionPenalty = 500;
    [SerializeField] private float driftScoreInterval = 0.25f;

    public int CurrentScore { get; private set; }

    private float driftTimer;

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
        DriftDetector detector =
            FindFirstObjectByType<DriftDetector>();

        if (detector == null || !detector.IsDrifting)
            return;

        DriftScoreStrategy strategy =
            new DriftScoreStrategy(detector.DriftAngle);

        AddScore(strategy.CalculateScore());
    }

    public void AwardLapBonus()
    {
        ApplyStrategy(new LapScoreStrategy(lapBonus));
    }

    public void ApplyCollisionPenalty()
    {
        ApplyStrategy(
            new CollisionPenaltyStrategy(collisionPenalty));
    }

    private void ApplyStrategy(ScoringStrategy strategy)
    {
        AddScore(strategy.CalculateScore());
    }

    private void AddScore(int amount)
    {
        CurrentScore = Mathf.Max(0, CurrentScore + amount);
        GameEvents.RaiseScoreChanged(CurrentScore);

        Debug.Log("Current score: " + CurrentScore);
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        driftTimer = 0f;
        GameEvents.RaiseScoreChanged(CurrentScore);
    }
}
