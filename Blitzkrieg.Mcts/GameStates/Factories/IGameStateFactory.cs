namespace Blitzkrieg.Mcts.GameStates.Factories
{
    public interface IGameStateFactory<T> where T:IGameState
    {
        T FromJson(string json);
        T FromInstance(T instance);
    }
}
