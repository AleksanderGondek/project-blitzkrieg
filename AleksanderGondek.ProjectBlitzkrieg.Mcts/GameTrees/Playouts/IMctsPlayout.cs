using System.Collections.Generic;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Handlers;

namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Playouts
{
    public interface IMctsPlayout<T, U> where T : class, IMctsNode, new() where U: class, IGameState, new()
    {
        int MaximumIterations { get; set; }
        int MaxiumumSimulations { get; set; }
        U GameState { get; set; }
        IMctsNodeHandler<T, U> NodeHandler { get; set; }

        IDictionary<string, int> GetNextMove();
    }
}
