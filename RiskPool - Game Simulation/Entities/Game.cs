using App.Core.Enums;

using RiskPool.GameSimulation.Enums;

namespace RiskPool.GameSimulation.Entities;

public class Game {
    public Game() {
    }

    public Game(Guid id, int roundsQuantity) {
        Id = id;
        RoundsQuantity = roundsQuantity;
    }

    public Guid Id { get; set; }
    public int RoundsQuantity { get; set; }
    public decimal RoundSum { get; set; }
    public decimal RewardSum { get; set; }

    public GameStateTypes StateId { get; set; }
    public GameResultTypes ResultId { get; set; }

    public List<GameRound> Rounds { get; set; } = new();
}