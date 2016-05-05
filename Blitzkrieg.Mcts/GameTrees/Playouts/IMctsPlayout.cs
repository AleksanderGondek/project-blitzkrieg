using Blitzkrieg.Mcts.GameStates;
using Blitzkrieg.Mcts.GameTrees.Handlers;

namespace Blitzkrieg.Mcts.GameTrees.Playouts
{
    public interface IMctsPlayout<T, U> where T : class, IMctsNode, new() where U: class, IGameState, new()
    {
        int MaximumIterations { get; set; }
        int MaxiumumSimulations { get; set; }
        U GameState { get; set; }
        IMctsNodeHandler<T, U> NodeHandler { get; set; }

        string GetNextMove();
    }
}
