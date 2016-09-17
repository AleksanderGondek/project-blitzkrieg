using System.Linq;
using System.Threading.Tasks;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Contracts;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Workers;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Examples.Chess;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Factories;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Factories;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Handlers;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Playouts;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.Persistence.Repositories;
using Orleans;

namespace AleksanderGondek.ProjectBlitzkrieg.Grains.Workers
{
    public class MctsSerialWithUctWoker : Grain, IMctsWorker
    {
        //TODO: This should be generic
        private IGameStateFactory<ChessGameState> _gameStateFactory;
        private IMctsNodeFactory<MctsNode, ChessGameState> _mctsNodeFactory;
        private IMctsNodeHandler<MctsNode, ChessGameState> _mctsNodeHandler;

        private void Initialize()
        {
            if (_gameStateFactory == null)
            {
                _gameStateFactory = new GameStateFactory<ChessGameState>();
            }

            if (_mctsNodeFactory == null)
            {
                _mctsNodeFactory = new MctsNodeFactory<MctsNode, ChessGameState>()
                {
                    GameStateFactory = _gameStateFactory
                };
            }

            if (_mctsNodeHandler == null)
            {
                _mctsNodeHandler = new MctsWithUctNodeHandler<MctsNode, ChessGameState>()
                {
                    DataBroker = new InMemoryDocumentRepository<MctsNode>(),
                    NodeFactory = _mctsNodeFactory
                };
            }
        }

        public Task<string> GetNextMove(ProcessingRequest request)
        {
            Initialize();

            var gameState = Newtonsoft.Json.JsonConvert.DeserializeObject<ChessGameState>(request.GameState);

            var playout = new DefaultSerialPlayout<MctsNode, ChessGameState>
            {
                NodeHandler = _mctsNodeHandler,
                GameState = gameState,
                MaximumIterations = 40,
                MaxiumumSimulations = 40
            };

            var possibleMovesWithScores = playout.GetNextMove();
            // Yield move that was most frequently taken
            return Task.FromResult(possibleMovesWithScores.Single(x => x.Value == possibleMovesWithScores.Values.Max()).Key);
        }
    }
}
