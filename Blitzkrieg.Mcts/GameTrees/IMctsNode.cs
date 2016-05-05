using System.Collections.Generic;
using Blitzkrieg.Mcts.GameStates;

namespace Blitzkrieg.Mcts.GameTrees
{
    public interface IMctsNode
    {
        // Metadata
        string Id { get; set; }
        string Type { get; set; }
        string Version { get; set; }
        string GameStateVersionCompatibility { get; set; }
        string MctsNamespace { get; set; }
        string HashType { get; set; }
        string Hash { get; }

        // Nodes connected
        string Parent { get; set; }
        IList<string> Children {get; set;}

        // Important data
        decimal Value { get; set; }
        int Visits { get; set; }

        // Action from which this node was created
        string OriginAction { get; set; }

        IGameState GameState { get; set; }

        void Initialize(string idOverride = null, string typeOverride = null, string versionOverride = null,
            string compatibilityOverride = null, string namespaceOverride = null, string hashTypeOverride = null, IGameState gameStateOverride = null);
        void IsValid();

        string ToJson();
    }
}
