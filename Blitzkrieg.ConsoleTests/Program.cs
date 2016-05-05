using System.IO;
using Blitzkrieg.Mcts.GameStates;
using Blitzkrieg.Mcts.GameStates.Examples.Chess;
using Blitzkrieg.Mcts.GameStates.Factories;
using Newtonsoft.Json;

namespace Blitzkrieg.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var newGameState = new ChessGameState();
            newGameState.Initialize();
            newGameState.IsValid();

            File.WriteAllText(@"newGameState.json", newGameState.ToJson());

            var fileLoaded = File.ReadAllText(@"newGameState.json");
            var chessGameStateFactory = new GameStateFactory<ChessGameState>();
            var loadedGameState = chessGameStateFactory.FromJson(fileLoaded);

            var loadedGameStateMetadata = JsonConvert.DeserializeObject<GameStateMetadataModel>(fileLoaded, new JsonSerializerSettings());
        }
    }
}
