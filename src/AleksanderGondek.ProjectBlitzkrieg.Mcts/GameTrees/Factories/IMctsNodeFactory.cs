using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Factories;

namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Factories
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
