using System;
using System.Collections.Generic;
using System.Linq;
using Blitzkrieg.Mcts.GameStates.Examples.Chess.Models;

namespace Blitzkrieg.Mcts.GameStates.Examples.Chess
{
    //TODO: Switch to char++ and char-- to Decrement / Increment
    public partial class ChessGameState
    {
        private string GetCurrentPlayer()
        {
            return LastPlayer == null ? null : AllPlayers.Except(new[] {LastPlayer}).First();
        }

        private char DecrementChar(char toBeDecremented, int times)
        {
            while (times > 0)
            {
                --toBeDecremented;
                --times;
            }

            return toBeDecremented;
        }

        private char IncrementChar(char toBeIncremented, int times)
        {
            while (times > 0)
            {
                ++toBeIncremented;
                --times;
            }

            return toBeIncremented;
        }

        private decimal GetBoardPiecesValueForPlayer(string player)
        {
            if (player == null)
            {
                return 0.0m;
            }

            var allPlayerPieces = _gameBoard.Where(x => x.Value.Owner == player).Select(y => y.Value.ChessPiece).ToList();
            var pawnsValue = allPlayerPieces.Count(x => x == BoardDefinitions.ChessPieces.Pawn) * 1.0m;
            var rooksValue = allPlayerPieces.Count(x => x == BoardDefinitions.ChessPieces.Rook) * 2.0m;
            var knightsValue = allPlayerPieces.Count(x => x == BoardDefinitions.ChessPieces.Knight) * 1.5m;
            var bishopsValue = allPlayerPieces.Count(x => x == BoardDefinitions.ChessPieces.Bishop) * 2.5m;
            var kingValue = allPlayerPieces.Count(x => x == BoardDefinitions.ChessPieces.King) * 4.0m;
            var queenValue = allPlayerPieces.Count(x => x == BoardDefinitions.ChessPieces.Queen) * 3.0m;
            return pawnsValue + rooksValue + knightsValue + bishopsValue + kingValue + queenValue;
        }

        private IList<string> GetPawnThreatFields(string coordiates)
        {
            var initialLetter = coordiates.Split(',').First().ToCharArray().First();
            var intialIndex = int.Parse(coordiates.Skip(1).First().ToString());
            var threatFields = new List<string>();

            // If first player, => whites => at the bottom => A1, etc.
            if (AllPlayers.IndexOf(GetCurrentPlayer()) == 0)
            {
                if (DecrementChar(initialLetter, 1) >= 'A' && intialIndex + 1 <= 8)
                {
                    threatFields.Add($"{DecrementChar(initialLetter, 1)}{intialIndex + 1}");
                }
                if (IncrementChar(initialLetter, 1) <= 'H' && intialIndex + 1 <= 8)
                {
                    threatFields.Add($"{IncrementChar(initialLetter, 1)}{intialIndex + 1}");
                }
            }
            // If second player, => black => at the top => A8, etc.
            else
            {
                if (DecrementChar(initialLetter, 1) >= 'A' && intialIndex - 1 >= 1)
                {
                    threatFields.Add($"{DecrementChar(initialLetter, 1)}{intialIndex - 1}");
                }
                if (IncrementChar(initialLetter, 1) <= 'H' && intialIndex - 1 >= 1)
                {
                    threatFields.Add($"{DecrementChar(initialLetter, 1)}{intialIndex - 1}");
                }
            }

            return threatFields;
        } 

        private IList<string> GetAllActionsForPlayer(string player)
        {
            var availableActions = new List<string>();
            var positionsOnBoard = _gameBoard.Where(x => x.Value.Owner == player).Select(x => x.Key).ToList();

            foreach (var coordinate in positionsOnBoard)
            {
                var letterIndex = coordinate.First();
                var numberIndex = int.Parse(coordinate[1].ToString());

                ChessField currentPositionData;
                _gameBoard.TryGetValue($"{letterIndex}{numberIndex}", out currentPositionData);
                if (currentPositionData == null)
                {
                    throw new Exception("Attempted to predict moves for non-existing pawn");
                }
                switch (currentPositionData.ChessPiece)
                {
                    case BoardDefinitions.ChessPieces.Pawn:
                        availableActions.AddRange(PawnActionsAvailable(letterIndex, numberIndex, currentPositionData));
                        break;
                    case BoardDefinitions.ChessPieces.Rook:
                        availableActions.AddRange(RookActionsAvailable(letterIndex, numberIndex, currentPositionData));
                        break;
                    case BoardDefinitions.ChessPieces.Knight:
                        availableActions.AddRange(KnightActionsAvailable(letterIndex, numberIndex, currentPositionData));
                        break;
                    case BoardDefinitions.ChessPieces.Bishop:
                        availableActions.AddRange(BishopActionsAvailable(letterIndex, numberIndex, currentPositionData));
                        break;
                    case BoardDefinitions.ChessPieces.Queen:
                        availableActions.AddRange(QueenActionsAvailable(letterIndex, numberIndex, currentPositionData));
                        break;
                    case BoardDefinitions.ChessPieces.King:
                        availableActions.AddRange(KingActionsAvailable(letterIndex, numberIndex, currentPositionData));
                        break;
                }
            }

            return availableActions;
        }

        private void HandlePawnMove(string currentPosition, string targetPosition, IList<string> possibleMoves, ChessField currentPositionData)
        {
            // Pawn is moving forward if it stays in the same letter column
            var isPawnMovingForward = currentPosition.First() == targetPosition.First();

            ChessField targetPositionData;
            // If field is occupied
            if (_gameBoard.TryGetValue(targetPosition, out targetPositionData))
            {
                // If field is occupied by enemy player, but we are not considering pawn moving forward
                if (targetPositionData.Owner != currentPositionData.Owner && !isPawnMovingForward)
                {
                    possibleMoves.Add($"{currentPosition},{targetPosition}");
                }
                return;
            }

            // Do not add as possible move (pawn can move diagonaly only if it can bash other piece
            if (isPawnMovingForward)
            {
                possibleMoves.Add($"{currentPosition},{targetPosition}");
            }
        }

        private bool HandleMoveToField(string currentPosition, string targetPosition, IList<string> possibleMoves,  ChessField currentPositionData, bool pawnForwardMove = false)
        {
            ChessField boardField;
            // If field is occupied
            if (_gameBoard.TryGetValue(targetPosition, out boardField))
            {
                // If field is occupied by enemy player, but we are not considering pawn moving forward
                if (boardField.Owner != currentPositionData.Owner && !pawnForwardMove)
                {
                    possibleMoves.Add($"{currentPosition},{targetPosition}");
                }

                return true;
            }

            possibleMoves.Add($"{currentPosition},{targetPosition}");
            return false;
        }

        private IList<string> RookActionsAvailable(char letterIndex, int numberIndex, ChessField currentPositionData)
        {
            var possibleMoves = new List<string>();

            // Check avaiable moves "down" from current position
            for (var i = numberIndex - 1; i >= 1; i--)
            {
                if (HandleMoveToField($"{letterIndex}{numberIndex}", $"{letterIndex}{i}", possibleMoves, currentPositionData))
                {
                    break;
                }
            }
            // Check avaiable moves "up" from current position
            for (var i = numberIndex + 1; i <= 8; i++)
            {
                if (HandleMoveToField($"{letterIndex}{numberIndex}", $"{letterIndex}{i}", possibleMoves, currentPositionData))
                {
                    break;
                }
            }

            // Check avaiable moves "left" from current position
            foreach (
                var letter in
                    BoardDefinitions.GetAlphabeticIndex()
                        .Take(BoardDefinitions.GetAlphabeticIndex().IndexOf(letterIndex)))
            {
                if (HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{numberIndex}", possibleMoves, currentPositionData))
                {
                    break;
                }
            }

            // Check avaiable moves "right" from current position
            var numberToSkip = BoardDefinitions.GetAlphabeticIndex().IndexOf(letterIndex) + 1;
            foreach (
                var letter in
                    BoardDefinitions.GetAlphabeticIndex()
                        .Skip(numberToSkip)
                        .Take(BoardDefinitions.GetAlphabeticIndex().Count - numberToSkip))
            {
                if (HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{numberIndex}", possibleMoves, currentPositionData))
                {
                    break;
                }
            }

            return possibleMoves;
        }

        private IList<string> KnightActionsAvailable(char letterIndex, int numberIndex, ChessField currentPositionData)
        {
            var possibleMoves = new List<string>();

            // Check avaiable moves "down" from current position
            var letter = letterIndex;
            if (numberIndex - 2 >= 1)
            {
                --letter;
                // Check move on the 'left'
                if (letter >= 'A')
                {
                    HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{numberIndex - 2}", possibleMoves, currentPositionData);
                }

                // Check move on the 'right'
                letter = letterIndex;
                ++letter;
                if (letter <= 'H')
                {
                    HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{numberIndex - 2}", possibleMoves, currentPositionData);
                }
            }

            // Check avaiable moves "up" from current position
            if (numberIndex + 2 <= 8)
            {
                // Check move on the 'left'
                letter = letterIndex;
                --letter;
                if (letter >= 'A')
                {
                    HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{numberIndex + 2}", possibleMoves, currentPositionData);
                }

                // Check move on the 'right'
                letter = letterIndex;
                ++letter;
                if (letter <= 'H')
                {
                    HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{numberIndex + 2}", possibleMoves, currentPositionData);
                }
            }

            // Check avaiable moves "left" from current position
            letter = letterIndex;
            --letter;
            --letter;
            if (letter >= 'A')
            {
                // Check move 'down'
                if (numberIndex - 1 >= 1)
                {
                    HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{numberIndex - 1}", possibleMoves, currentPositionData);
                }

                // Check move 'up'

                if (numberIndex + 1 <= 8)
                {
                    HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{numberIndex + 1}", possibleMoves, currentPositionData);
                }
            }

            // Check avaiable moves "right" from current position
            letter = letterIndex;
            ++letter;
            ++letter;
            if (letter <= 'H')
            {
                // Check move 'down'
                if (numberIndex - 1 >= 1)
                {
                    HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{numberIndex - 1}", possibleMoves, currentPositionData);
                }

                // Check move 'up'
                if (numberIndex + 1 <= 8)
                {
                    HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{numberIndex + 1}", possibleMoves, currentPositionData);
                }
            }

            return possibleMoves;
        }

        private IList<string> BishopActionsAvailable(char letterIndex, int numberIndex, ChessField currentPositionData)
        {
            var possibleMoves = new List<string>();
            // Down-left
            var letter = letterIndex;
            var number = numberIndex;
            while (--letter >= 'A' && --number >= 1)
            {
                if (HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData))
                {
                    break;
                }
            }

            // Down-right
            letter = letterIndex;
            number = numberIndex;
            while (++letter <= 'H' && --number >= 1)
            {
                if (HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData))
                {
                    break;
                }
            }

            // Up-left
            letter = letterIndex;
            number = numberIndex;
            while (--letter >= 'A' && ++number <= 8)
            {
                if (HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData))
                {
                    break;
                }
            }

            // Up-right
            letter = letterIndex;
            number = numberIndex;
            while (++letter <= 'H' && ++number <= 8)
            {
                if (HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData))
                {
                    break;
                }
            }

            return possibleMoves;
        }

        private IList<string> QueenActionsAvailable(char letterIndex, int numberIndex, ChessField currentPositionData)
        {
            var possibleMoves = new List<string>();

            //Left
            var letter = letterIndex;
            var number = numberIndex;
            while (--letter >= 'A')
            {
                if (HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData))
                {
                    break;
                }
            }

            // Up-left
            letter = letterIndex;
            number = numberIndex;
            while (--letter >= 'A' && ++number <= 8)
            {
                if (HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData))
                {
                    break;
                }
            }

            //Up
            letter = letterIndex;
            number = numberIndex;
            while (++number <= 8)
            {
                if (HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData))
                {
                    break;
                }
            }

            // Up-right
            letter = letterIndex;
            number = numberIndex;
            while (++letter <= 'H' && ++number <= 8)
            {
                if (HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData))
                {
                    break;
                }
            }

            //Right
            letter = letterIndex;
            number = numberIndex;
            while (++letter <= 'H')
            {
                if (HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData))
                {
                    break;
                }
            }

            // Down-right
            letter = letterIndex;
            number = numberIndex;
            while (++letter <= 'H' && --number >= 1)
            {
                if (HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData))
                {
                    break;
                }
            }

            // Down
            letter = letterIndex;
            number = numberIndex;
            while (--number >= 1)
            {
                if (HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData))
                {
                    break;
                }
            }

            // Down-left
            letter = letterIndex;
            number = numberIndex;
            while (--letter >= 'A' && --number >= 1)
            {
                if (HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData))
                {
                    break;
                }
            }

            return possibleMoves;
        }

        private IList<string> KingActionsAvailable(char letterIndex, int numberIndex, ChessField currentPositionData)
        {
            var possibleMoves = new List<string>();

            //Left
            var letter = letterIndex;
            var number = numberIndex;
            if (--letter >= 'A')
            {
                HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData);
            }

            // Up-left
            letter = letterIndex;
            number = numberIndex;
            if (--letter >= 'A' && ++number <= 8)
            {
                HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData);
            }

            //Up
            letter = letterIndex;
            number = numberIndex;
            if (++number <= 8)
            {
                HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData);
            }

            // Up-right
            letter = letterIndex;
            number = numberIndex;
            if (++letter <= 'H' && ++number <= 8)
            {
                HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData);
            }

            //Right
            letter = letterIndex;
            number = numberIndex;
            if (++letter <= 'H')
            {
                HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData);
            }

            // Down-right
            letter = letterIndex;
            number = numberIndex;
            if (++letter <= 'H' && --number >= 1)
            {
                HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData);
            }

            // Down
            letter = letterIndex;
            number = numberIndex;
            if (--number >= 1)
            {
                HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData);
            }

            // Down-left
            letter = letterIndex;
            number = numberIndex;
            if (--letter >= 'A' && --number >= 1)
            {
                HandleMoveToField($"{letterIndex}{numberIndex}", $"{letter}{number}", possibleMoves, currentPositionData);
            }

            return possibleMoves;
        }

        private IList<string> PawnActionsAvailable(char letterIndex, int numberIndex, ChessField currentPositionData)
        {
            var possibleMoves = new List<string>();

            // If first player, => whites => at the bottom => A1, etc.
            if (AllPlayers.IndexOf(currentPositionData.Owner) == 0)
            {
                int newNumberIndex;
                if (currentPositionData.IsStartingPosition)
                {
                    newNumberIndex = numberIndex + 2;
                    if (newNumberIndex <= 8)
                    {
                        HandlePawnMove($"{letterIndex}{numberIndex}", $"{letterIndex}{newNumberIndex}", possibleMoves, currentPositionData);
                    }

                }
                // Move Forward
                newNumberIndex = numberIndex + 1;
                if (newNumberIndex <= 8)
                {
                    HandlePawnMove($"{letterIndex}{numberIndex}", $"{letterIndex}{newNumberIndex}", possibleMoves, currentPositionData);
                }

                //Move diagonaly
                var letterIndexCopy = letterIndex;
                var newLetterIndex = --letterIndexCopy;
                if (newLetterIndex >= 'A' && newNumberIndex <= 8)
                {
                    HandlePawnMove($"{letterIndex}{numberIndex}", $"{newLetterIndex}{newNumberIndex}", possibleMoves, currentPositionData);
                }
                letterIndexCopy = letterIndex;
                newLetterIndex = ++letterIndexCopy;
                if (newLetterIndex <= 'H' && newNumberIndex <= 8)
                {
                    HandlePawnMove($"{letterIndex}{numberIndex}", $"{newLetterIndex}{newNumberIndex}", possibleMoves, currentPositionData);
                }
            }
            // If second player, => black => at the top => A8, etc.
            else
            {
                int newNumberIndex;
                if (currentPositionData.IsStartingPosition)
                {
                    newNumberIndex = numberIndex - 2;
                    if (newNumberIndex >= 1)
                    {
                        HandlePawnMove($"{letterIndex}{numberIndex}", $"{letterIndex}{newNumberIndex}", possibleMoves, currentPositionData);
                    }
                }
                // Move Forward
                newNumberIndex = numberIndex - 1;
                if (newNumberIndex >= 1)
                {
                    HandlePawnMove($"{letterIndex}{numberIndex}", $"{letterIndex}{newNumberIndex}", possibleMoves, currentPositionData);
                }

                //Move diagonaly
                var letterIndexCopy = letterIndex;
                var newLetterIndex = --letterIndexCopy;
                if (newLetterIndex >= 'A' && newNumberIndex >= 1)
                {
                    HandlePawnMove($"{letterIndex}{numberIndex}", $"{newLetterIndex}{newNumberIndex}", possibleMoves, currentPositionData);
                }

                letterIndexCopy = letterIndex;
                newLetterIndex = ++letterIndexCopy;
                if (newLetterIndex <= 'H' && newNumberIndex >= 1)
                {
                    HandlePawnMove($"{letterIndex}{numberIndex}", $"{newLetterIndex}{newNumberIndex}", possibleMoves, currentPositionData);
                }
            }

            return possibleMoves;
        }

        private bool AreThereAnyChecks(IList<string> possibleMoves, out IList<string> threatingPositions)
        {
            // There should never be a situation where one player does not have a king
            var kingPosition = _gameBoard.Single(x => x.Value.ChessPiece == BoardDefinitions.ChessPieces.King && x.Value.Owner == GetCurrentPlayer()).Key;

            // If there are moves that threaten current players king, return them and notify that they are present
            threatingPositions = possibleMoves.Where(x => x.Split(',').Skip(1).First() == kingPosition).ToList();
            return threatingPositions.Any();
        }
    }
}
