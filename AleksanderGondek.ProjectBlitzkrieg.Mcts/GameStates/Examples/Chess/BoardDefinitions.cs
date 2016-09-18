using System.Collections.Generic;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Examples.Chess.Models;

namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Examples.Chess
{
    public static class BoardDefinitions
    {
        public enum ChessPieces { Pawn, Knight, Bishop, Rook, Queen, King, None };

        public static IList<char> GetAlphabeticIndex()
        {
            return "ABCDEFGH".ToCharArray();
        } 

        public static IDictionary<string, ChessField> GetDefaultBoard(IList<string> players)
        {
            if (players.Count < 2)
            {
                return new Dictionary<string, ChessField>();
            }

            return new Dictionary<string, ChessField>
            {
                {"A1", new ChessField {ChessPiece = ChessPieces.Rook, Owner = players[0], IsStartingPosition = true }},
                {"B1", new ChessField {ChessPiece = ChessPieces.Knight, Owner = players[0], IsStartingPosition = true }},
                {"C1", new ChessField {ChessPiece = ChessPieces.Bishop, Owner = players[0], IsStartingPosition = true }},
                {"D1", new ChessField {ChessPiece = ChessPieces.Queen, Owner = players[0], IsStartingPosition = true }},
                {"E1", new ChessField {ChessPiece = ChessPieces.King, Owner = players[0], IsStartingPosition = true }},
                {"F1", new ChessField {ChessPiece = ChessPieces.Bishop, Owner = players[0], IsStartingPosition = true }},
                {"G1", new ChessField {ChessPiece = ChessPieces.Knight, Owner = players[0], IsStartingPosition = true }},
                {"H1", new ChessField {ChessPiece = ChessPieces.Rook, Owner = players[0], IsStartingPosition = true }},
                {"A2", new ChessField {ChessPiece = ChessPieces.Pawn, Owner = players[0], IsStartingPosition = true }},
                {"B2", new ChessField {ChessPiece = ChessPieces.Pawn, Owner = players[0], IsStartingPosition = true }},
                {"C2", new ChessField {ChessPiece = ChessPieces.Pawn, Owner = players[0], IsStartingPosition = true }},
                {"D2", new ChessField {ChessPiece = ChessPieces.Pawn, Owner = players[0], IsStartingPosition = true }},
                {"E2", new ChessField {ChessPiece = ChessPieces.Pawn, Owner = players[0], IsStartingPosition = true }},
                {"F2", new ChessField {ChessPiece = ChessPieces.Pawn, Owner = players[0], IsStartingPosition = true }},
                {"G2", new ChessField {ChessPiece = ChessPieces.Pawn, Owner = players[0], IsStartingPosition = true }},
                {"H2", new ChessField {ChessPiece = ChessPieces.Pawn, Owner = players[0], IsStartingPosition = true }},
                {"A8", new ChessField {ChessPiece = ChessPieces.Rook, Owner = players[1], IsStartingPosition = true }},
                {"B8", new ChessField {ChessPiece = ChessPieces.Knight, Owner = players[1], IsStartingPosition = true }},
                {"C8", new ChessField {ChessPiece = ChessPieces.Bishop, Owner = players[1], IsStartingPosition = true }},
                {"D8", new ChessField {ChessPiece = ChessPieces.Queen, Owner = players[1], IsStartingPosition = true }},
                {"E8", new ChessField {ChessPiece = ChessPieces.King, Owner = players[1], IsStartingPosition = true }},
                {"F8", new ChessField {ChessPiece = ChessPieces.Bishop, Owner = players[1], IsStartingPosition = true }},
                {"G8", new ChessField {ChessPiece = ChessPieces.Knight, Owner = players[1], IsStartingPosition = true }},
                {"H8", new ChessField {ChessPiece = ChessPieces.Rook, Owner = players[1], IsStartingPosition = true }},
                {"A7", new ChessField {ChessPiece = ChessPieces.Pawn, Owner = players[1], IsStartingPosition = true }},
                {"B7", new ChessField {ChessPiece = ChessPieces.Pawn, Owner = players[1], IsStartingPosition = true }},
                {"C7", new ChessField {ChessPiece = ChessPieces.Pawn, Owner = players[1], IsStartingPosition = true }},
                {"D7", new ChessField {ChessPiece = ChessPieces.Pawn, Owner = players[1], IsStartingPosition = true }},
                {"E7", new ChessField {ChessPiece = ChessPieces.Pawn, Owner = players[1], IsStartingPosition = true }},
                {"F7", new ChessField {ChessPiece = ChessPieces.Pawn, Owner = players[1], IsStartingPosition = true }},
                {"G7", new ChessField {ChessPiece = ChessPieces.Pawn, Owner = players[1], IsStartingPosition = true }},
                {"H7", new ChessField {ChessPiece = ChessPieces.Pawn, Owner = players[1], IsStartingPosition = true }}
            };
        } 
    }
}
