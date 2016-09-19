using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates;

namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Factories
{
    public class MctsNodeFactoryForSharedTree<T, U> : MctsNodeFactory<T, U> where T : class, IMctsNode, new() where U : class, IGameState, new()
    {
        public new T FromIGameState(U gameState)
        {
            var node = new T();
            var gameStateCopy = GameStateFactory.FromInstance(gameState);

            node.Initialize(idOverride:GenerateSharableNodeId(node, gameStateCopy), gameStateOverride:gameStateCopy);
            node.ActionsNotTaken = node.GameState.AvailableActions();
            node.IsValid();

            return node;
        }

        private string GenerateSharableNodeId(T node, U gameState)
        {
            // Should be shared between workers sharing namespace, type, version. 
            // Hash represents uniquie gamestate so it should repeat itself
            return $"{node.MctsNamespace}:{node.Type}:{node.Version}:{gameState.Hash}";
        }
    }
}
