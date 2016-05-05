using System;
using System.Linq;
using Blitzkrieg.Mcts.GameStates;
using Blitzkrieg.Mcts.GameTrees.Handlers;

namespace Blitzkrieg.Mcts.GameTrees.Playouts
{
    public class DefaultSerialPlayout<T,U> : IMctsPlayout<T, U> where T : class, IMctsNode, new() where U : class, IGameState, new()
    {
        public int MaximumIterations { get; set; }
        public int MaxiumumSimulations { get; set; }
        public U GameState { get; set; }
        public IMctsNodeHandler<T, U> NodeHandler { get; set; } 

        private int _iterationsCount = 0;
        private T _currentNode;

        public string GetNextMove()
        {
            _currentNode = NodeHandler.NewNodeFromGameState(GameState);

            while (_iterationsCount < MaximumIterations)
            {
                SelectionStep();
                ExpansionStep();
                SimulationStep();
                BackpropagationStep();

                _iterationsCount++;
            }

            var rootChildWithMostVisits = NodeHandler.GetChildNodeWithMostVisits(_currentNode);
            return rootChildWithMostVisits != null ? rootChildWithMostVisits.OriginAction : string.Empty;
        }

        private void SelectionStep()
        {
            // Do while currentNode has no available actions (all were performed) and currentNode has chilrden to select from
            while (!_currentNode.GameState.AvailableActions().Any() && _currentNode.Children.Any())
            {
                _currentNode = NodeHandler.SelectChildNode(_currentNode);
            }
        }

        private void ExpansionStep()
        {
            if (!_currentNode.GameState.AvailableActions().Any())
            {
                return;
            }

            var actionsAvailable = _currentNode.GameState.AvailableActions();
            //TODO: later switch to more random new RNGCryptoServiceProvider
            var randomIndex = new Random(new DateTime().Millisecond).Next(0, actionsAvailable.Count);
            var randomAction = actionsAvailable.ElementAt(randomIndex);

            // New state
            _currentNode = NodeHandler.NewNodeFromPerformingAction(_currentNode, randomAction);

        }

        private void SimulationStep()
        {
            var simulationsCount = 0;
            var actionsAvailable = _currentNode.GameState.AvailableActions();
            while (!actionsAvailable.Any())
            {
                var randomIndex = new Random(new DateTime().Millisecond).Next(0, actionsAvailable.Count);
                var randomAction = actionsAvailable.ElementAt(randomIndex);

                // New state
                _currentNode = NodeHandler.NewNodeFromPerformingAction(_currentNode, randomAction);
                actionsAvailable = _currentNode.GameState.AvailableActions();

                simulationsCount++;
                if (simulationsCount > MaxiumumSimulations)
                {
                    break;
                }
            }
        }

        private void BackpropagationStep()
        {
            while (_currentNode.Parent != null)
            {
                _currentNode = NodeHandler.UpdateNodeValueAndGetParent(_currentNode);
            }
        }
    }
}
