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

        private static Random _random = new Random((int)DateTime.UtcNow.Ticks);
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
            NodeHandler.DataBroker.CleanAll();
            return rootChildWithMostVisits != null ? rootChildWithMostVisits.OriginAction : string.Empty;
        }

        private void SelectionStep()
        {
            // Do while currentNode has no available actions (all were performed) and currentNode has chilrden to select from
            while (!_currentNode.ActionsNotTaken.Any() && _currentNode.Children.Any())
            {
                _currentNode = NodeHandler.SelectChildNode(_currentNode);
            }
        }

        private void ExpansionStep()
        {
            if (!_currentNode.ActionsNotTaken.Any())
            {
                return;
            }

            var actionsAvailable = _currentNode.ActionsNotTaken;
            //TODO: later switch to more random new RNGCryptoServiceProvider
            var randomIndex = _random.Next(0, actionsAvailable.Count);
            var randomAction = actionsAvailable.ElementAt(randomIndex);

            // New state
            _currentNode = NodeHandler.NewNodeFromPerformingAction(_currentNode, randomAction);
        }

        private void SimulationStep()
        {
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
