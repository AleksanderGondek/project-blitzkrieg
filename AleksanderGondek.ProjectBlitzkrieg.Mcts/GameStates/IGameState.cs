using System.Collections.Generic;

namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates
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

        void Initialize(string versionOverride = null, string hashTypeOverride = null, IList<string> playersOverride = null, string lastPlayer = null, object metaData = null);
        void IsValid();
        IList<string> AvailableActions();
        void PerformAction(string action);
        string ToJson(); 
    }
}
