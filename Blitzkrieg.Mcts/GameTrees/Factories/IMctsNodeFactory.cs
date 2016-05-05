using Blitzkrieg.Mcts.GameStates;
using Blitzkrieg.Mcts.GameStates.Factories;

namespace Blitzkrieg.Mcts.GameTrees.Factories
{
    public interface IMctsNodeFactory<T, U> where T: class, IMctsNode, new() where U : class, IGameState, new()
    {
        IGameStateFactory<U> GameStateFactory { get; set; }
        T FromJson(string json);
        T FromInstance(T instance);
        T FromIGameState(U gameState);
        T NewNode();
    }
}
