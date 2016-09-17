namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Factories
{
    public interface IGameStateFactory<T> where T: class, IGameState, new()
    {
        T FromJson(string json);
        T FromInstance(T instance);
        T NewGameState();
    }
}
