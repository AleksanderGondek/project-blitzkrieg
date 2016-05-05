using System.Collections.Generic;
using Newtonsoft.Json;

namespace Blitzkrieg.Mcts.GameStates
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GameStateMetadataModel
    {
        [JsonProperty]
        public string Version { get; set; }
        [JsonProperty]
        public string Type { get; set; }
        [JsonProperty]
        public IList<string> AllPlayers { get; set; }
        [JsonProperty]
        public string LastPlayer { get; set; }
        [JsonProperty]
        public string Hash { get; }
        [JsonProperty]
        public string HashType { get; set; }
    }
}
