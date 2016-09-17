using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Examples.Chess;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Examples.Chess.Models;

using Newtonsoft.Json;

namespace AleksanderGondek.ProjectBlitzkrieg.LocalRunners
{
    public class HelperModel
    {
        [JsonProperty("GameBoard")]
        public IDictionary<string, ChessField> GameBoard { get; set; }
        [JsonProperty("AllPlayers")]
        public IList<string> AllPlayers { get; set; }
    }

    public static class Helper
    {
        private static string GetPlayerSymbol(ChessField field, IList<string> allPlayers)
        {
            return allPlayers.IndexOf(field.Owner).ToString();
        }
        private static string GetChessPieceSymbol(ChessField field)
        {
            switch (field.ChessPiece)
            {
                case BoardDefinitions.ChessPieces.Pawn:
                    return "P";
                case BoardDefinitions.ChessPieces.Rook:
                    return "R";
                case BoardDefinitions.ChessPieces.Bishop:
                    return "B";
                case BoardDefinitions.ChessPieces.Knight:
                    return "K";
                case BoardDefinitions.ChessPieces.Queen:
                    return "Q";
                case BoardDefinitions.ChessPieces.King:
                    return "V";
                default:
                    return "N";
            }
        }

        public static void PrintoutBoard(IGameState gameState)
        {
            var helperModel = JsonConvert.DeserializeObject<HelperModel>(gameState.ToJson(), new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });

            var builder = new StringBuilder();
            foreach (var number in Enumerable.Range(0, 9))
            {
                if (number == 0)
                {
                    builder.Append("[ ] | [A] | [B] | [C] | [D] | [E] | [F] | [G] | [H]");
                    Console.WriteLine(builder.ToString());
                    builder.Clear();
                    continue;
                }

                foreach (var letter in "ABCDEFGH".ToCharArray())
                {
                    if (letter == 'A')
                    {
                        builder.Append($"[{number}] |");
                    }

                    ChessField field;
                    helperModel.GameBoard.TryGetValue($"{letter}{number}", out field);
                    if (letter != 'H')
                    {
                        builder.Append(field != null ? $" {GetPlayerSymbol(field, helperModel.AllPlayers)}:{GetChessPieceSymbol(field)} |" : " [ ] |");
                    }
                    else
                    {
                        builder.Append(field != null ? $" {GetPlayerSymbol(field, helperModel.AllPlayers)}:{GetChessPieceSymbol(field)}" : " [ ]");
                    }

                }

                Console.WriteLine(builder.ToString());
                builder.Clear();
            }
        }
    }
}
