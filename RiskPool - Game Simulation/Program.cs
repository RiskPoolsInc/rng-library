using RiskPool.GameSimulation;
using RiskPool.GameSimulation.Entities;

Console.WriteLine("Hello in RiskPool simulation");
var exitGame = false;
var games = new List<Game>();
var gameService = new GameService();

do {
    Console.ForegroundColor = ConsoleColor.White;
    var game = gameService.CreateNewGame();
    gameService.RunGame(game).Wait();

    Console.WriteLine("Start new game? Enter y/n");
    var repeatGame = Console.ReadLine();
    exitGame = repeatGame.ToLower() != "y";
} while (!exitGame);

Console.WriteLine("Simulation is completed");
Task.Delay(2000).Wait();