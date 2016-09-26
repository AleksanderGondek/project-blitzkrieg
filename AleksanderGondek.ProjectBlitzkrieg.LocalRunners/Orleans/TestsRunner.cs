using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Brokers;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Contracts;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Examples.Chess;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Factories;
using Orleans;

namespace AleksanderGondek.ProjectBlitzkrieg.LocalRunners.Orleans
{
    public class TestConfiguration
    {
        public string GameState { get; set; }
        public string ExectutionType { get; set; }
        public int Workers { get; set; }
        public int MaximumIterations { get; set; }
        public int MaxiumumSimulations { get; set; }

        public int Repetitions { get; set; }

        public string Winner { get; set; }
        public string TimeTaken { get; set; }
    }


    public static class TestsRunner
    {
        public static int Repetitions = 1;
        public static int MaximumWorkers = 2;
        public static int MaximumIterations = 40;
        public static int MaximumSimulations = 80;

        private static Guid _playerBrokerGuid = Guid.NewGuid();

        private static IGameStateFactory<ChessGameState> _chessGameStateFactory = new GameStateFactory<ChessGameState>();

        private static IGameState _defaultGameState;

        public static void Run(string executionType)
        {
            RunConfiguration(GetConfigurationToRun(executionType));
        }

        private static async void RunConfiguration(TestConfiguration configuration)
        {
            var beginingGameSTate = configuration.GameState;

            var playerBroker = GrainClient.GrainFactory.GetGrain<IMctsBroker>(_playerBrokerGuid);
            var workerGuid = Guid.NewGuid();

            for (var i = 0; i < configuration.Repetitions; i++)
            {
                var gameState = _chessGameStateFactory.FromJson(beginingGameSTate);
                configuration.GameState = gameState.ToJson();

                var stopWatch = new Stopwatch();
                stopWatch.Start();
                while (gameState.AvailableActions().Any())
                {
                    var request = GetRequestFromConfiguration(configuration, gameState.AllPlayers.Except(new[] { gameState.LastPlayer }).First());
                    request.TargetGrain = workerGuid;
                    var move = await playerBroker.GetNextMove(request);
                    Console.WriteLine(move);

                    gameState.PerformAction(move);
                    
                    Helper.PrintoutBoard(gameState);

                    configuration.GameState = gameState.ToJson();
                }

                stopWatch.Stop();
                configuration.TimeTaken = stopWatch.Elapsed.ToString("g");
                configuration.Winner = gameState.LastPlayer;
            }

            System.IO.File.WriteAllText($"{DateTime.UtcNow:yy-MM-dd-hh-mm-ss}-shared.txt",
                $"{configuration.ExectutionType}, " +
                $"{configuration.Winner}, " +
                $"{configuration.Workers}, " +
                $"{configuration.MaximumIterations}, " +
                $"{configuration.MaxiumumSimulations}" +
                $"{configuration.TimeTaken}");
        }

        private static ProcessingRequest GetRequestFromConfiguration(TestConfiguration configuration, string currentPlayer)
        {
            var request = new ProcessingRequest()
            {
                GameState = configuration.GameState,
                ExectutionType = currentPlayer == "Test_Player_One" ? configuration.ExectutionType : AvailableExecutionTypes.MctsSerialWithUtc,
                MaximumIterations = configuration.MaximumIterations,
                MaxiumumSimulations = configuration.MaxiumumSimulations,
                Workers = configuration.Workers
            };

            Console.WriteLine($"Will send request with execution type of: {request.ExectutionType}");
            return request;
        }

        private static string GetDefaultGameStateJson()
        {
            if (_defaultGameState == null)
            {
                _defaultGameState = new ChessGameState();
                _defaultGameState.Initialize(metaData:BoardDefinitions.GetDifficultBoard(new List<string> { "Test_Player_One", "Test_Player_Two" }));
                _defaultGameState.IsValid();
            }

            return _defaultGameState.ToJson();
        }

        private static TestConfiguration GetConfigurationToRun(string executionType)
        {
            return new TestConfiguration()
            {
                GameState = GetDefaultGameStateJson(),
                ExectutionType = executionType,
                MaximumIterations = MaximumIterations,
                MaxiumumSimulations = MaximumSimulations,
                Workers = MaximumWorkers,
                Repetitions = Repetitions,
                TimeTaken = string.Empty,
                Winner = string.Empty
            };
        }
    }
}
