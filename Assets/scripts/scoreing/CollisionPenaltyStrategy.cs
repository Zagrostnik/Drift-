
public class CollisionPenaltyStrategy : ScoringStrategy
{
    private readonly int penalty;

    public CollisionPenaltyStrategy(int penalty)
    {
        this.penalty = penalty;
    }

    public override int CalculateScore()
    {
        return -penalty;
    }
}
