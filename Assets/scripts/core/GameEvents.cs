
using System;

public static class GameEvents
{
    public static event Action<int> ScoreChanged;

    public static void RaiseScoreChanged(int score)
    {
        ScoreChanged?.Invoke(score);
    }
}
