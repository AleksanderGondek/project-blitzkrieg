using System;
using System.Linq;
using AleksanderGondek.ProjectBlitzkrieg.LocalRunners.Orleans;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Examples.Chess;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Factories;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Factories;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Handlers;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Playouts;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.Persistence.Repositories;

namespace AleksanderGondek.ProjectBlitzkrieg.LocalRunners
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //TestMcts();
            OrleansSilo.Start();   
        }

        public static void TestMcts()
        {
            // Serial Mcts example
            var chessGameStateFactory = new GameStateFactory<ChessGameState>();
            var mctsNodeFactory = new MctsNodeFactory<MctsNode, ChessGameState>
            {
                GameStateFactory = chessGameStateFactory
            };
            var handler = new MctsWithUctNodeHandler<MctsNode, ChessGameState>
            {
                DataBroker = new InMemoryDocumentRepository<MctsNode>(),
                NodeFactory = mctsNodeFactory
            };

            var newGameState = new ChessGameState();
            newGameState.Initialize();
            newGameState.IsValid();

            while (newGameState.AvailableActions().Any())
            {
                Console.WriteLine("Before taking a move:");
                Helper.PrintoutBoard(newGameState);
                Console.WriteLine("-------------");

                //For futre puproses
                var currentPlayer = newGameState.LastPlayer == null ? null : newGameState.AllPlayers.Except(new[] { newGameState.LastPlayer }).First();
                var playout = new DefaultSerialPlayout<MctsNode, ChessGameState>
                {
                    NodeHandler = handler,
                    GameState = newGameState,
                    MaximumIterations = 40,
                    MaxiumumSimulations = 40
                };

                var possibleMovesWithScores = playout.GetNextMove();
                var action = possibleMovesWithScores.First(x => x.Value == possibleMovesWithScores.Values.Max()).Key;
                newGameState.PerformAction(action);

                Console.WriteLine($"After taking a move({action}):");
                Helper.PrintoutBoard(newGameState);
                Console.WriteLine("-------------");
            }

            Console.ReadLine();
        }
    }
}
