using System.Collections.Generic;
using Blitzkrieg.Mcts.GameStates;
using Newtonsoft.Json;

namespace Blitzkrieg.Mcts.GameTrees
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MctsNodeMetadataModel : IMctsNode
    {
        [JsonProperty]
        public string Id { get; set; }
        [JsonProperty]
        public string Type { get; set; }
        [JsonProperty]
        public string Version { get; set; }
        [JsonProperty]
        public string GameStateVersionCompatibility { get; set; }
        [JsonProperty]
        public string MctsNamespace { get; set; }
        [JsonProperty]
        public string HashType { get; set; }
        [JsonProperty]
        public string Hash { get; }

        [JsonProperty]
        public string Parent { get; set; }
        [JsonProperty]
        public IList<string> Children { get; set; }

        [JsonProperty]
        public decimal Value { get; set; }
        [JsonProperty]
        public int Visits { get; set; }

        [JsonProperty]
        public string OriginAction { get; set; }

        [JsonProperty]
        public IGameState GameState { get; set; }

        public void Initialize(string idOverride = null, string typeOverride = null, string versionOverride = null,
            string compatibilityOverride = null, string namespaceOverride = null, string hashTypeOverride = null, IGameState gameStateOverride = null)
        {}

        public void IsValid(){}
        public string ToJson()
        {
            return string.Empty;
        }
    }
}
