namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Examples.Chess.Models
{
    public class ChessField
    {
        public BoardDefinitions.ChessPieces ChessPiece { get; set; }
        public bool IsStartingPosition { get; set; }
        public string Owner { get; set; }
    }
}
