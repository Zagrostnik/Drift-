
using UnityEngine;

public class LapManager : MonoBehaviour
{
    [SerializeField] private int totalLaps = 3;
    [SerializeField] private int checkpointCount = 4;

    private int nextCheckpointIndex;
    private int completedLaps;

    public int CompletedLaps => completedLaps;

    public void RegisterCheckpoint(int checkpointIndex)
    {
        if (GameManager.Instance == null ||
            GameManager.Instance.State != GameState.Playing)
            return;

        if (checkpointIndex != nextCheckpointIndex)
            return;

        nextCheckpointIndex++;

        if (nextCheckpointIndex >= checkpointCount)
        {
            nextCheckpointIndex = 0;
            completedLaps++;

            ScoreManager scoreManager =
                FindFirstObjectByType<ScoreManager>();

            if (scoreManager != null)
            {
                scoreManager.AwardLapBonus();
            }

            Debug.Log(
                $"Круг {completedLaps}/{totalLaps} завершён!");

            if (completedLaps >= totalLaps)
            {
                Debug.Log("Все круги пройдены!");

                GameManager.Instance.FinishRace();

                Debug.Log(
                    "Текущее состояние: " +
                    GameManager.Instance.State
                );
            }
        }
    }

    public void ResetLaps()
    {
        nextCheckpointIndex = 0;
        completedLaps = 0;

        Debug.Log("Круги сброшены");
    }

}
