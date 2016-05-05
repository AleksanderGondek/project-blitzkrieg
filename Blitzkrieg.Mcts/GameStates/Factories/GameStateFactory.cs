using Newtonsoft.Json;

namespace Blitzkrieg.Mcts.GameStates.Factories
{
    public class GameStateFactory<T> : IGameStateFactory<T> where T : class, IGameState, new()
    {
        public T FromJson(string json)
        {
            //TODO: Better error handling
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });
        }

        public T FromInstance(T instance)
        {
            return FromJson(instance.ToJson());
        }

        public T NewGameState()
        {
            var state = new T();
            state.Initialize();
            state.IsValid();
            return state;
        }
    }
}
