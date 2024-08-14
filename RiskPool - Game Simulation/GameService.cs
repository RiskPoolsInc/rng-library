using System.Security.Cryptography;

using App.Core.Enums;

using RiskPool.GameSimulation.Commands;
using RiskPool.GameSimulation.Entities;
using RiskPool.GameSimulation.Enums;

namespace RiskPool.GameSimulation;

public class GameService {
    public Game CreateNewGame() {
        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine("Starting game for base bet 1000");
        Console.ForegroundColor = ConsoleColor.White;
        var game = new Game(Guid.NewGuid(), 0);
        game.RoundSum = 1000;
        var roundsQuantityEntered = string.Empty;
        var roundsQuantityValidationFault = true;

        while (roundsQuantityValidationFault) {
            Console.WriteLine("Enter quantity game rounds from 3 to 10");
            roundsQuantityEntered = Console.ReadLine();
            roundsQuantityValidationFault = !int.TryParse(roundsQuantityEntered, out var roundsQuantity);

            if (roundsQuantityValidationFault && roundsQuantity >= 3 && roundsQuantity <= 10)
                Console.WriteLine("Incorrect game round quantity");
            else
                game.RoundsQuantity = roundsQuantity;
        }

        return game;
    }

    public async Task RunGame(Game currentGame) {
        var currentGameId = currentGame.Id;
        var gameCounter = 1;
        var gameLose = false;
        Console.WriteLine("Game is running...");

        for (var i = 0; i < currentGame.RoundsQuantity; i++) {
            var roundNumber = i + 1;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Starting game round {roundNumber}/{currentGame.RoundsQuantity}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Ganarating number...");
            var generatedNumber = await GenerateNextRandomNumber();
            Console.WriteLine($"Ganarated number: {generatedNumber}");

            var roundResult = (generatedNumber % 2) == 0 ? GameRoundResultTypes.Lose : GameRoundResultTypes.Win;

            if (roundResult == GameRoundResultTypes.Win)
                gameCounter++;
            else
                gameCounter--;

            var currentRoundSum = gameCounter * currentGame.RoundSum;

            var createGameRoundCommand = new CreateGameRoundCommand(currentGameId, generatedNumber, roundResult, currentRoundSum,
                roundNumber);

            AddGameRound(currentGame, createGameRoundCommand);

            if (gameCounter <= 0) {
                gameLose = true;
                GameIsLose(currentGame);
                break;
            }
        }

        if (!gameLose)
            GameIsWin(currentGame, gameCounter);
    }

    private void GameIsLose(Game currentGame) {
        currentGame.StateId = GameStateTypes.Completed;
        currentGame.ResultId = GameResultTypes.Lose;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Game is lose");
        Console.ForegroundColor = ConsoleColor.White;
    }

    private void GameIsWin(Game currentGame, int gameCounter) {
        currentGame.StateId = GameStateTypes.Completed;
        currentGame.ResultId = GameResultTypes.Win;
        currentGame.RewardSum = currentGame.RoundSum * gameCounter;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Game is win");
        Console.WriteLine($"You win {currentGame.RewardSum} coins");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
    }

    private void AddGameRound(Game currentGame, CreateGameRoundCommand createGameRoundCommand) {
        var gameRound = new GameRound {
            Id = Guid.NewGuid(),
            GameId = currentGame.Id,
            Number = createGameRoundCommand.GeneratedNumber,
            CurrentGameRoundSum = createGameRoundCommand.CurrentGameRoundSum,
            RoundNumber = createGameRoundCommand.RoundNumber,
            Result = createGameRoundCommand.Result,
        };
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Generated game round {gameRound.Id.ToString("D")}");
        Console.ForegroundColor = gameRound.Result == GameRoundResultTypes.Win ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine($"Round is {gameRound.Result}");
        Console.ForegroundColor = ConsoleColor.Blue;
        currentGame.Rounds.Add(gameRound);
        Console.Write("Game rounds:");
        Console.Write("\t");

        foreach (var currentGameRound in currentGame.Rounds) {
            Console.ForegroundColor = currentGameRound.Result == GameRoundResultTypes.Win ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($"{currentGameRound.RoundNumber}/{currentGame.RoundsQuantity}");
            Console.Write(" ");
            Console.Write(currentGameRound.Number);
            Console.Write(" ");
            Console.Write(currentGameRound.Result);
            Console.Write(" (");
            Console.Write(currentGameRound.CurrentGameRoundSum);
            Console.Write(")");
            Console.Write("\t");
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;
    }

    private async Task RandomDelay(int from, int to) {
        var random = new Random();
        var next = random.Next(from, to);
        await Task.Delay(next);
    }

    private async Task<int> GenerateNextRandomNumber() {
        await RandomDelay(500, 1000);
        var firstRandomNumber = RandomNumberGenerator.GetInt32(2, 5000);
        await RandomDelay(1800, 2300);
        var secondRandomNumber = RandomNumberGenerator.GetInt32(6000, 10000);
        await RandomDelay(1800, 2300);

        var orderedRandomNumbers = new[] {
                firstRandomNumber,
                secondRandomNumber
            }.OrderBy(a => a)
             .ToArray();
        await RandomDelay(50, 100);

        var generatedRandomNumber = RandomNumberGenerator.GetInt32(orderedRandomNumbers[0], orderedRandomNumbers[1]);

        return generatedRandomNumber;
    }
}