
using UnityEngine;

public class DriftScoreStrategy : ScoringStrategy
{
    private readonly float driftAngle;

    public DriftScoreStrategy(float driftAngle)
    {
        this.driftAngle = driftAngle;
    }

    public override int CalculateScore()
    {
        float angleBonus = Mathf.Clamp(driftAngle, 0f, 60f);
        return Mathf.RoundToInt(angleBonus);
    }
}
