using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Contracts;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Workers;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Handlers;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Playouts;
using Orleans;

namespace AleksanderGondek.ProjectBlitzkrieg.Grains.Workers.CustomizedPlayouts
{
    public class LeafParallelizationPlayout<T, U> : IMctsPlayout<T, U> where T : class, IMctsNode, new() where U : class, IGameState, new()
    {
        public int MaximumIterations { get; set; }
        public int MaxiumumSimulations { get; set; }
        public U GameState { get; set; }

        public int Workers { get; set; }

        public IMctsNodeHandler<T, U> NodeHandler { get; set; }
        public IGrainFactory GrainFactory { get; set; }


        private static Random _random = new Random((int)DateTime.UtcNow.Ticks);
        private int _iterationsCount = 0;
        private T _currentNode;

        public IDictionary<string, int> GetNextMove()
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

            var possibleMovesWithScore = NodeHandler.GetPossibleMovesWithScore(_currentNode);
            return possibleMovesWithScore;
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

        private async void SimulationStep()
        {
            var request = new ProcessingRequest()
            {
                GameState = _currentNode.GameState.ToJson(),
                ExectutionType = AvailableExecutionTypes.MctsLeafParallelizationWithUct,
                MaximumIterations = MaximumIterations,
                MaxiumumSimulations = MaxiumumSimulations,
                Workers = 1
            };

            var workersTasks = new List<Task<bool>>();
            for (var i = 0; i < request.Workers; i++)
            {
                var worker = GrainFactory.GetGrain<IMctsSimulationStepWorker>(Guid.NewGuid());
                workersTasks.Add(worker.PerformSimulation(request));
            }
            var allTasks = Task.WhenAll(workersTasks);
            await allTasks;
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
