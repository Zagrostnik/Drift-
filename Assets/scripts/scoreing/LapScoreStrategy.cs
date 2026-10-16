
public class LapScoreStrategy : ScoringStrategy
{
    private readonly int lapBonus;

    public LapScoreStrategy(int lapBonus)
    {
        this.lapBonus = lapBonus;
    }

    public override int CalculateScore()
    {
        return lapBonus;
    }
}
