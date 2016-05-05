using Blitzkrieg.Mcts.GameStates;
using Blitzkrieg.Mcts.GameTrees.Factories;
using Blitzkrieg.Mcts.Persistence.Contracts;

namespace Blitzkrieg.Mcts.GameTrees.Handlers
{
    public interface IMctsNodeHandler<T, U> where T : class, IMctsNode, new() where U : class, IGameState, new()
    {
        IMctsNodeFactory<T, U> NodeFactory { get; set; }
        IDefaultDocumentContract<T> DataBroker { get; set; }

        T SelectChildNode(T nodeFromWhichSelect);
        T NewNodeFromGameState(U gameState);
        T NewNodeFromPerformingAction(T nodeParent, string action);
        T UpdateNodeValueAndGetParent(T node);
        T GetChildNodeWithMostVisits(T node);
        void MergeWith(T nodeToMergeTo, string nodeToMergeWithId);
    }
}
