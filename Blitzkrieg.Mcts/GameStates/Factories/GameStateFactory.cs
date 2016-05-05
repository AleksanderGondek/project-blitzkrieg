using Newtonsoft.Json;

namespace Blitzkrieg.Mcts.GameStates.Factories
{
    public class GameStateFactory<T> : IGameStateFactory<T> where T : IGameState
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
    }
}
