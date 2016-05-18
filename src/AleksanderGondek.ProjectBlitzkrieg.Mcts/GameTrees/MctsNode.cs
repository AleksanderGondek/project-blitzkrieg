using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Exceptions;
using Newtonsoft.Json;

namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MctsNode : IMctsNode
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
        public string Hash => GetHash();

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
        public IList<string> ActionsNotTaken { get; set; }

        [JsonProperty]
        public IGameState GameState { get; set; }

        public void Initialize(string idOverride = null, string typeOverride = null, string versionOverride = null, 
            string compatibilityOverride = null, string namespaceOverride = null, string hashTypeOverride = null, IGameState gameStateOverride = null)
        {
            Type = typeOverride ?? GetType().FullName;
            Version = versionOverride ?? "1.0.0";
            GameStateVersionCompatibility = compatibilityOverride ?? "1.0.0";
            MctsNamespace = namespaceOverride ?? "blitzkrieg-mcts-node";
            HashType = hashTypeOverride ?? "SHA256";
            Id = idOverride ?? $"{MctsNamespace}:{Type}:{Version}:{Guid.NewGuid().ToString("N")}";

            Value = 0.0m;
            Visits = 0;

            Children = new List<string>();
            GameState = gameStateOverride;
        }

        public void IsValid()
        {
            IList<string> listOfIssues = new List<string>();
            if (GameState == null)
            {
                listOfIssues.Add(@"Invalid MctsNode:IMctsNode GameState - provided state has to be non-null.");
            }
            if (string.IsNullOrEmpty(Type))
            {
                listOfIssues.Add($"Invalid MctsNode:IMctsNode Type - {Type} was given, non-empty string was expected.");
            }
            try
            {
                new Version(Version);
            }
            catch (Exception)
            {
                listOfIssues.Add($"Invalid MctsNode:IMctsNode Version - {Version} was given, major.minor.build.revision scheme was expected.");
            }
            try
            {
                new Version(GameStateVersionCompatibility);
            }
            catch (Exception)
            {
                listOfIssues.Add($"Invalid MctsNode:IMctsNode GameStateVersionCompatibility - {GameStateVersionCompatibility} was given, major.minor.build.revision scheme was expected.");
            }
            if (string.IsNullOrEmpty(MctsNamespace))
            {
                listOfIssues.Add($"Invalid MctsNode:IMctsNode MctsNamespace - {MctsNamespace} was given, non-empty string was expected.");
            }
            if (string.IsNullOrEmpty(HashType))
            {
                listOfIssues.Add($"Invalid MctsNode:IMctsNode HashType - {HashType} was given, non-empty string was expected.");
            }
            if (Children == null)
            {
                listOfIssues.Add($"Invalid MctsNode:IMctsNode Children - is null, IList<string> was expected.");
            }
            if (!Regex.IsMatch(Id, @"[^:]\b(\S+):(\S+):(\S+)\b"))
            {
                listOfIssues.Add($"Invalid MctsNode:IMctsNode Id - {Id} was given, string in lorem:ipsum:Ipsum1:loreM format was expected.");
            }

            if (listOfIssues.Any())
            {
                throw new MctsNodeNotValid(@"MctsNode:IMctsNode is not valid!") { ValidationIssues = listOfIssues };
            }
        }

        public string ToJson()
        {
            //TODO: Better error handling
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        private string GetHash()
        {
            var sha256Hasher = SHA256.Create();
            var stringPayload = $"{MctsNamespace}|{Type}|{Version}|{GameState.ToJson()}";
            var byteRepresentation = Encoding.UTF8.GetBytes(stringPayload);
            var byteHash = sha256Hasher.ComputeHash(byteRepresentation);
            var hash = new StringBuilder(byteHash.Length * 2);
            foreach (var t in byteHash)
            {
                hash.Append(t.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
