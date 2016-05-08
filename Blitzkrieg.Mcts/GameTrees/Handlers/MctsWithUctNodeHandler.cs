using System;
using System.Collections.Generic;
using System.Linq;
using Blitzkrieg.Mcts.GameStates;
using Blitzkrieg.Mcts.GameTrees.Factories;
using Blitzkrieg.Mcts.GameTrees.Utilities;
using Blitzkrieg.Mcts.Persistence.Contracts;

namespace Blitzkrieg.Mcts.GameTrees.Handlers
{
    public class MctsWithUctNodeHandler<T, U> : IMctsNodeHandler<T, U> where T : class, IMctsNode, new() where U : class, IGameState, new()
    {
        public IMctsNodeFactory<T, U> NodeFactory { get; set; }
        public IDefaultDocumentContract<T> DataBroker { get; set; }
        public T SelectChildNode(T nodeFromWhichSelect)
        {
            var nodeChildren = nodeFromWhichSelect.Children.Select(childId => DataBroker.Get(childId)).ToList();
            //TODO: check if really sorted as should be
            nodeChildren.Sort(NodeComparators.CompareWithBasicUcb);
            // Biggest should be first
            return nodeChildren.First();
        }

        public T NewNodeFromGameState(U gameState)
        {
            var newNode = NodeFactory.FromIGameState(gameState);
            return DataBroker.Store(newNode) ? newNode : null;
        }

        public T NewNodeFromPerformingAction(T nodeParent, string action)
        {
            // As U statement should be always true
            var stateAfterAction = NodeFactory.GameStateFactory.FromInstance(nodeParent.GameState as U);
            stateAfterAction.PerformAction(action);

            // As U statement should be always true
            var newNode = NodeFactory.FromIGameState(nodeParent.GameState as U);
            newNode.Parent = nodeParent.Id;
            newNode.OriginAction = action;
            if (!DataBroker.Store(newNode))
            {
                return null;
            }

            nodeParent.ActionsNotTaken.Remove(action);
            nodeParent.Children.Add(newNode.Id);
            if (!DataBroker.Update(nodeParent))
            {
                return null;
            }

            return newNode;
        }

        public T UpdateNodeValueAndGetParent(T node)
        {
            node.Visits = node.Visits + 1;
            // TODO: This make not be greatest idea
            node.Value = node.Value + node.GameState.StateValue;

            if (!DataBroker.Update(node))
            {
                return null;
            }

            return DataBroker.Get(node.Parent);
        }

        public T GetChildNodeWithMostVisits(T node)
        {
            var nodeChildren = node.Children.Select(childId => DataBroker.Get(childId)).ToList();
            return nodeChildren.First(x => x.Visits == nodeChildren.Max(y => y.Visits));
        }

        public void MergeWith(T nodeToMergeTo, string nodeToMergeWithId)
        {
            throw new NotImplementedException();
        }
    }
}
