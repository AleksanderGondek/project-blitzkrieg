using System.Collections.Generic;

namespace Blitzkrieg.Mcts.GameStates
{
    public interface IGameState
    {
        string Version { get; set; }
        string Type { get; set; }
        IList<string> AllPlayers { get; set; }
        string LastPlayer { get; set; }
        decimal StateValue { get; }
        // 'Zobrist' Hash
        string Hash { get; }
        string HashType { get; set; }

        void Initialize(string versionOverride, string hashTypeOverride, IList<string> playersOverride, string lastPlayer, object metaData);
        void IsValid();
        IList<string> AvailableActions();
        void PerformAction(string action);
        string ToJson(); 
    }
}
