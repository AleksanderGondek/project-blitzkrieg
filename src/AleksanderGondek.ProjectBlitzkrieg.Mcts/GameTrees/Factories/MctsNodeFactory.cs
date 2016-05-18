using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Factories;
using Newtonsoft.Json;

namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Factories
{
    public class MctsNodeFactory<T, U> : IMctsNodeFactory<T, U> where T: class, IMctsNode, new() where U : class, IGameState, new()
    {
        public IGameStateFactory<U> GameStateFactory { get; set; }

        public T FromJson(string json)
        {
            //TODO: Better error handling
            var deserialized = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });
            deserialized.IsValid();
            return deserialized;
        }

        public T FromInstance(T instance)
        {
            return FromJson(instance.ToJson());
        }

        public T FromIGameState(U gameState)
        {
            var node = new T();
            node.Initialize(gameStateOverride:GameStateFactory.FromInstance(gameState));
            node.ActionsNotTaken = node.GameState.AvailableActions();
            node.IsValid();
            return node;
        }

        public T NewNode()
        {
            var node = new T();
            node.Initialize();
            node.IsValid();
            return node;
        }
    }
}
