using RiskPool.GameSimulation.Enums;

namespace RiskPool.GameSimulation.Commands;

public class CreateGameRoundCommand {
    public Guid GameId { get; set; }
    public int GeneratedNumber { get; set; }
    public decimal CurrentGameRoundSum { get; set; }
    public int RoundNumber { get; set; }
    public GameRoundResultTypes Result { get; set; }

    public CreateGameRoundCommand() {
    }

    public CreateGameRoundCommand(Guid gameId, int generatedNumber, GameRoundResultTypes result, decimal currentGameRoundSum,
                                  int  roundNumber) {
        GameId = gameId;
        GeneratedNumber = generatedNumber;
        CurrentGameRoundSum = currentGameRoundSum;
        RoundNumber = roundNumber;
        Result = result;
    }
}