using RiskPool.GameSimulation.Enums;

namespace RiskPool.GameSimulation.Entities;

public class GameRound {
    public Guid Id { get; set; }
    public Guid GameId { get; set; }

    public int Number { get; set; }
    public decimal CurrentGameRoundSum { get; set; }
    public int RoundNumber { get; set; }
    public GameRoundResultTypes Result { get; set; }
    public string Hash { get; set; }
}