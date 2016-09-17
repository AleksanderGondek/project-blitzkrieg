using System.Collections.Generic;
using Newtonsoft.Json;

namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GameStateMetadataModel : IGameState
    {
        [JsonProperty]
        public string Version { get; set; }
        [JsonProperty]
        public string Type { get; set; }
        [JsonProperty]
        public IList<string> AllPlayers { get; set; }
        [JsonProperty]
        public string LastPlayer { get; set; }

        [JsonIgnore]
        public decimal StateValue { get; }

        [JsonProperty]
        public string Hash { get; }
        [JsonProperty]
        public string HashType { get; set; }

        public void Initialize(string versionOverride = null, string hashTypeOverride = null, IList<string> playersOverride = null,
            string lastPlayer = null, object metaData = null){}
        public void IsValid(){}
        public IList<string> AvailableActions()
        {
            return null;
        }
        public void PerformAction(string action){}
        public string ToJson()
        {
            return string.Empty;
        }
    }
}
