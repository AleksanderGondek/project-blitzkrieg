using System;
using System.Linq;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Handlers;

namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Playouts
{
    public class SimulationOnlyPlayout<T, U> where T : class, IMctsNode, new() where U : class, IGameState, new()
    {
        public int MaxiumumSimulations { get; set; }
        public U GameState { get; set; }
        public IMctsNodeHandler<T, U> NodeHandler { get; set; }

        private T _currentNode;

        public bool PerformSimulation()
        {
            _currentNode = NodeHandler.NewNodeFromGameState(GameState);
            var startingNodeId = _currentNode.Id;

            var simulationsCount = 0;
            while (_currentNode.ActionsNotTaken.Any())
            {
                var randomIndex = new Random(new DateTime().Millisecond).Next(0, _currentNode.ActionsNotTaken.Count);
                var randomAction = _currentNode.ActionsNotTaken.ElementAt(randomIndex);

                // New state
                _currentNode = NodeHandler.NewNodeFromPerformingAction(_currentNode, randomAction);

                simulationsCount++;
                if (simulationsCount > MaxiumumSimulations)
                {
                    break;
                }
            }

            while (_currentNode.Parent != startingNodeId)
            {
                _currentNode = NodeHandler.UpdateNodeValueAndGetParent(_currentNode);
            }

            return true;
        }
    }
}
