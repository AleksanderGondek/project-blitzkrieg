using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Examples.Chess.Models;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Exceptions;
using Newtonsoft.Json;

namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Examples.Chess
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class ChessGameState : IGameState
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
        public decimal StateValue => GetStateValue();

        [JsonProperty]
        public string Hash => GetHashState();

        [JsonProperty]
        public string HashType { get; set; }

        [JsonProperty("GameBoard", Order=1)]
        private IDictionary<string, ChessField> _gameBoard;


        public void Initialize(string versionOverride = null, string hashTypeOverride = null, IList<string> playersOverride = null, string lastPlayer = null, object metaData = null)
        {
            // For convinance
            Type = GetType().FullName;
            // Semantic versioning
            Version = versionOverride ?? "1.0.0";
            HashType = hashTypeOverride ?? "SHA256";
            AllPlayers = playersOverride ?? new List<string> {"Test_Player_One", "Test_Player_Two"};
            LastPlayer = lastPlayer ?? AllPlayers.Last();

            IDictionary<string, ChessField> gameBoardOverride = metaData as IDictionary<string, ChessField>;
            _gameBoard = gameBoardOverride ?? BoardDefinitions.GetDefaultBoard(AllPlayers);
        }

        public void IsValid()
        {
            IList<string> listOfIssues = new List<string>();
            if (Type != GetType().FullName)
            {
                listOfIssues.Add($"Invalid ChessGameState:IGameState Type - {GetType().FullName} was expected, {Type} was given.");
            }
            try
            {
                new Version(Version);
            }
            catch (Exception)
            {
                listOfIssues.Add($"Invalid ChessGameState:IGameState Version - {Version} was given, major.minor.build.revision scheme was expected.");
            }
            if (string.IsNullOrEmpty(HashType))
            {
                listOfIssues.Add($"Invalid ChessGameState:IGameState HashType - {HashType} was given, non-empty string was expected.");
            }
            if (AllPlayers.Count != 2)
            {
                listOfIssues.Add($"Invalid ChessGameState:IGameState AllPlayers - 2 players were expected, {AllPlayers.Count} were given.");
            }
            if (string.IsNullOrEmpty(LastPlayer))
            {
                listOfIssues.Add($"Invalid ChessGameState:IGameState LastPlayer -  {LastPlayer} was given, non-empty string was expected.");
            }

            if (listOfIssues.Any())
            {
                throw new StateNotValid("ChessGameState:IGameState is not valid!") { ValidationIssues = listOfIssues };
            }
        }

        public IList<string> AvailableActions()
        {
            var enemyPlayerActions = GetAllActionsForPlayer(AllPlayers.Single(x => x != GetCurrentPlayer()));
            var curremtPlayerActions = GetAllActionsForPlayer(AllPlayers.Single(x => x == GetCurrentPlayer()));

            //TODO: This is tmp fix
            if (
                !_gameBoard.Any(
                    x => x.Value.ChessPiece == BoardDefinitions.ChessPieces.King && x.Value.Owner == GetCurrentPlayer()))
            {
                return new List<string>();
            }

            //If There are only two kings left
            if (_gameBoard.Count(x => x.Value.ChessPiece == BoardDefinitions.ChessPieces.King) == _gameBoard.Count)
            {
                return new List<string>();
            }

            // If there are only 3 pawns this will take way too long
            if (_gameBoard.Count(x => x.Value.ChessPiece != BoardDefinitions.ChessPieces.Pawn) <= 3)
            {
                return new List<string>();
            }

            // Extract all positions that enemy may move to next turn
            var allEnemyMoves = enemyPlayerActions.Select(x => x.Split(',').Skip(1).First()).ToList();
            // Get All possible actions for current players King
            var kingPossibleActions =
                curremtPlayerActions.Where(
                    x =>
                        x.Split(',').First() ==
                        _gameBoard.Single(
                            y =>
                                y.Value.ChessPiece == BoardDefinitions.ChessPieces.King &&
                                y.Value.Owner == GetCurrentPlayer()).Key).ToList();
            // Get moves for that King that are resulting in stepping into fields the opposing player may go to
            var invalidKingMoves = kingPossibleActions.Where(x => allEnemyMoves.Contains(x.Split(',').Skip(1).First())).ToList();
            
            // Remove them from king Possible actions (we may need this if there is a check)
            kingPossibleActions = kingPossibleActions.Except(invalidKingMoves).ToList();
            // remove them from all possible actions
            curremtPlayerActions = curremtPlayerActions.Except(invalidKingMoves).ToList();

            // Get rid of moves that places king in the enemy pawn way
            invalidKingMoves.Clear();
            foreach (var action in kingPossibleActions)
            {
                var swagger = GetPawnThreatFields(action.Split(',').Skip(1).First());
                foreach (var neighbour in swagger)
                {
                    ChessField field;
                    if (_gameBoard.TryGetValue(neighbour, out field) && 
                        field.ChessPiece == BoardDefinitions.ChessPieces.Pawn && 
                        field.Owner != GetCurrentPlayer())
                    {
                        invalidKingMoves.Add(action);
                    }
                }
            }

            // Remove them from king Possible actions (we may need this if there is a check)
            kingPossibleActions = kingPossibleActions.Except(invalidKingMoves).ToList();
            // remove them from all possible actions
            curremtPlayerActions = curremtPlayerActions.Except(invalidKingMoves).ToList();
            invalidKingMoves.Clear();

            IList<string> threatingPositions;
            if (!AreThereAnyChecks(enemyPlayerActions, out threatingPositions)) return curremtPlayerActions;
            
            //If King can move or we can kill threating piece
            // List of moves that will kill the piece that is checking
            var test = threatingPositions.Select(x => x.Split(',').First()).ToList();
            var counterCheckMoves = curremtPlayerActions.Where(x => test.Contains(x.Split(',').Skip(1).First())).ToList();

            // Now to obtain the list of valid king moves
            // Combine possiblites
            counterCheckMoves.AddRange(kingPossibleActions);

            // Distinct values
            counterCheckMoves = counterCheckMoves.Distinct().ToList();

            //If returned list is empty it means endgame
            return counterCheckMoves;
        }

        public void PerformAction(string action)
        {
            var startingCooridnates = action.Split(',').First();
            var targetCooridnates = action.Split(',').Skip(1).First();

            ChessField startingFieldData;
            if (!_gameBoard.TryGetValue(startingCooridnates, out startingFieldData))
            {
                throw new Exception("Attempting to perform move on non-existing entity!");
            }

            _gameBoard.Remove(startingCooridnates);
            if (_gameBoard.ContainsKey(targetCooridnates))
            {
                _gameBoard.Remove(targetCooridnates);
            }

            _gameBoard.Add(targetCooridnates, startingFieldData);
            LastPlayer = GetCurrentPlayer();
        }
  
        public string ToJson()
        {
            //TODO: Better error handling
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        private string GetHashState()
        {
            var sha256Hasher = SHA256.Create();
            var stringPayload = $"{Type}|{Version}|{JsonConvert.SerializeObject(_gameBoard, Formatting.None)}";
            var byteRepresentation = Encoding.UTF8.GetBytes(stringPayload);
            var byteHash = sha256Hasher.ComputeHash(byteRepresentation);
            var hash = new StringBuilder(byteHash.Length * 2);
            foreach (var t in byteHash)
            {
                hash.Append(t.ToString("x2"));
            }
            return hash.ToString();
        }

        private decimal GetStateValue()
        {
            return GetBoardPiecesValueForPlayer(GetCurrentPlayer());
        }
    }
}
