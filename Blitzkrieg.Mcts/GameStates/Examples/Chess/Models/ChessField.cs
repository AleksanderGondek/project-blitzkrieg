using System;

namespace Blitzkrieg.Mcts.GameStates.Examples.Chess.Models
{
    public class ChessField : ICloneable
    {
        public BoardDefinitions.ChessPieces ChessPiece { get; set; }
        public bool IsStartingPosition { get; set; }
        public string Owner { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
