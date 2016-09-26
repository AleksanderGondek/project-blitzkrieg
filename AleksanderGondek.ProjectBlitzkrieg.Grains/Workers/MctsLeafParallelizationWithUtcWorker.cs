using System.Collections.Generic;
using System.Threading.Tasks;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Contracts;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Workers;
using AleksanderGondek.ProjectBlitzkrieg.Grains.Workers.CustomizedPlayouts;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Examples.Chess;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Factories;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Factories;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Handlers;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.Persistence.Repositories;
using Orleans;

namespace AleksanderGondek.ProjectBlitzkrieg.Grains.Workers
{
    public class MctsLeafParallelizationWithUtcWorker : Grain, IMctsLeafParallelizationWithUtcWorker
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
                _mctsNodeFactory = new MctsNodeFactoryForSharedTree<MctsNode, ChessGameState>()
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


        public async Task<IDictionary<string, int>> GetNextMove(ProcessingRequest request)
        {
            Initialize();
            var playout = new LeafParallelizationPlayout<MctsNode, ChessGameState>()
            {
                NodeHandler = _mctsNodeHandler,
                GameState = Newtonsoft.Json.JsonConvert.DeserializeObject<ChessGameState>(request.GameState),
                MaximumIterations = request.MaximumIterations,
                MaxiumumSimulations = request.MaxiumumSimulations,
                GrainFactory = GrainFactory
            };
            return playout.GetNextMove();
        }
    }
}
