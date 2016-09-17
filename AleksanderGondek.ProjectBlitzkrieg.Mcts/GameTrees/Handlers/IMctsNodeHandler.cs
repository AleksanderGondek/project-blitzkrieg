using System.Collections.Generic;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Factories;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.Persistence.Contracts;

namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Handlers
{
    public interface IMctsNodeHandler<T, U> where T : class, IMctsNode, new() where U : class, IGameState, new()
    {
        IMctsNodeFactory<T, U> NodeFactory { get; set; }
        IDefaultDocumentContract<T> DataBroker { get; set; }

        T SelectChildNode(T nodeFromWhichSelect);
        T NewNodeFromGameState(U gameState);
        T NewNodeFromPerformingAction(T nodeParent, string action);
        T UpdateNodeValueAndGetParent(T node);
        IDictionary<string, int> GetPossibleMovesWithScore(T node);
        void MergeWith(T nodeToMergeTo, string nodeToMergeWithId);
    }
}
