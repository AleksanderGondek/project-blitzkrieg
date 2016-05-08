using System;
using System.IO;
using System.Linq;
using Blitzkrieg.Mcts.GameStates;
using Blitzkrieg.Mcts.GameStates.Examples.Chess;
using Blitzkrieg.Mcts.GameStates.Factories;
using Blitzkrieg.Mcts.GameTrees;
using Blitzkrieg.Mcts.GameTrees.Factories;
using Blitzkrieg.Mcts.GameTrees.Handlers;
using Blitzkrieg.Mcts.GameTrees.Playouts;
using Blitzkrieg.Mcts.Persistence.Repositories;
using Newtonsoft.Json;

namespace Blitzkrieg.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
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

                var action = playout.GetNextMove();
                newGameState.PerformAction(action);

                Console.WriteLine($"After taking a move({action}):");
                Helper.PrintoutBoard(newGameState);
                Console.WriteLine("-------------");
            }

            Console.ReadLine();
        }
    }
}
