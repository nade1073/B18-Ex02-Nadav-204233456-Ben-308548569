namespace Program
{
    using System;
    using System.Collections.Generic;

    internal class IAChecker
    {
        public const int k_IADepth = 5;
        private readonly BoardSquareScore m_ScoresOfBoard;

        public IAChecker(eSizeBoard i_SizeOfBoard)
        {
            m_ScoresOfBoard = new BoardSquareScore(i_SizeOfBoard);
        }

        public SquareMove IACheckerCalculateNextMove(CheckerBoard i_CurrentCheckerBoard, List<AIMovementScore> i_ListOfAllMovements)
        {
            double alpha = Double.NegativeInfinity;
            double beta = Double.PositiveInfinity;
            CheckerBoard tempBoard = null;
            Boolean maxmizingPlayer = true;
            foreach (AIMovementScore currentMove in i_ListOfAllMovements)
            {
                performMoveAndSwitchPlayers(i_CurrentCheckerBoard, out tempBoard, currentMove.SquareMove);
                currentMove.ScoreOfMove = miniMaxAlgorithem(tempBoard, IAChecker.k_IADepth, !maxmizingPlayer, alpha, beta);
                currentMove.ScoreInBoard = m_ScoresOfBoard.ArrayOfScores[currentMove.ToSquare.Row - MovementOptions.k_StartRow, currentMove.ToSquare.Col - MovementOptions.k_StartCol];
            }

            double maxHeuristics = Double.NegativeInfinity;
            foreach (AIMovementScore currentMove in i_ListOfAllMovements)
            {
                if (currentMove.ScoreOfMove > maxHeuristics)
                {
                    maxHeuristics = currentMove.ScoreOfMove;
                }
            }

            i_ListOfAllMovements.RemoveAll(item => item.ScoreOfMove < maxHeuristics);
            if (i_ListOfAllMovements.Count > 0)
            {
                int maxOfScoreInBoard = int.MinValue;
                foreach (AIMovementScore currentMove in i_ListOfAllMovements)
                {
                    if (currentMove.ScoreInBoard > maxOfScoreInBoard)
                    {
                        maxOfScoreInBoard = currentMove.ScoreInBoard;
                    }
                }

                i_ListOfAllMovements.RemoveAll(item => item.ScoreInBoard < maxOfScoreInBoard);
            }

            Random rand = new Random();
            int randomIndex = rand.Next(i_ListOfAllMovements.Count);
            return i_ListOfAllMovements[randomIndex].SquareMove;
        }

        private double getHeuristic(CheckerBoard i_Board)
        {
            double kingWeight = 1.3;
            double result = 0;

            if (i_Board.CurrentPlayer.TypeOfPlayer == eTypeOfPlayer.Computer)
            {
                result = (i_Board.CurrentPlayer.getNumberOfSpesificSoldierType(eSoldierType.King) * kingWeight) + i_Board.CurrentPlayer.getNumberOfSpesificSoldierType(eSoldierType.Regular) - (i_Board.OtherPlayer.getNumberOfSpesificSoldierType(eSoldierType.King) * kingWeight) - i_Board.OtherPlayer.getNumberOfSpesificSoldierType(eSoldierType.Regular);
            }
            else
            {
                result = (i_Board.OtherPlayer.getNumberOfSpesificSoldierType(eSoldierType.King) * kingWeight) + i_Board.OtherPlayer.getNumberOfSpesificSoldierType(eSoldierType.Regular) - (i_Board.CurrentPlayer.getNumberOfSpesificSoldierType(eSoldierType.King) * kingWeight) - i_Board.CurrentPlayer.getNumberOfSpesificSoldierType(eSoldierType.Regular);
            }

            return result;
        }

        private Double miniMaxAlgorithem(CheckerBoard i_Board, int i_Depth, bool i_MaxmizingPlayer, double i_Alpha, double i_Beta)
        {
            if (i_Depth == 0)
            {
                return getHeuristic(i_Board);
            }

            List<SquareMove> availableVaildMoves;
            if (i_Board.SoliderThatNeedToEatNextTurn == null)
            {
                availableVaildMoves = i_Board.generateValidMovesOfPlayer(i_Board.CurrentPlayer);
            }
            else
            {
                availableVaildMoves = i_Board.getValidMoveOfSolider(i_Board.SoliderThatNeedToEatNextTurn);
            }

            double initial = 0;
            CheckerBoard tempBoard = null;
            if (i_MaxmizingPlayer)
            {
                initial = Double.NegativeInfinity;
                foreach (SquareMove currentMove in availableVaildMoves)
                {
                    double result = 0;
                    performMoveAndSwitchPlayers(i_Board, out tempBoard, currentMove);
                    if (tempBoard.SoliderThatNeedToEatNextTurn != null)
                    {
                        result = miniMaxAlgorithem(tempBoard, i_Depth - 1, i_MaxmizingPlayer, i_Alpha, i_Beta);
                    }
                    else
                    {
                        result = miniMaxAlgorithem(tempBoard, i_Depth - 1, !i_MaxmizingPlayer, i_Alpha, i_Beta);
                    }

                    initial = Math.Max(result, initial);
                    i_Alpha = Math.Max(i_Alpha, initial);
                    if (i_Alpha >= i_Beta)
                    {
                        break;
                    }
                }
            }
            else
            {
                initial = Double.PositiveInfinity;
                foreach (SquareMove currentMove in availableVaildMoves)
                {
                    double result = 0;
                    performMoveAndSwitchPlayers(i_Board, out tempBoard, currentMove);
                    if (tempBoard.SoliderThatNeedToEatNextTurn != null)
                    {
                        result = miniMaxAlgorithem(tempBoard, i_Depth - 1, i_MaxmizingPlayer, i_Alpha, i_Beta);
                    }
                    else
                    {
                        result = miniMaxAlgorithem(tempBoard, i_Depth - 1, !i_MaxmizingPlayer, i_Alpha, i_Beta);
                    }

                    initial = Math.Min(result, initial);
                    i_Beta = Math.Min(i_Beta, initial);
                    if (i_Alpha >= i_Beta)
                    {
                        break;
                    }
                }
            }

            return initial;
        }

        private void performMoveAndSwitchPlayers(CheckerBoard i_Original, out CheckerBoard o_CopyOfCheckerBoard, SquareMove i_SquareToMoveInNewBoard)
        {
            o_CopyOfCheckerBoard = new CheckerBoard(i_Original);
            o_CopyOfCheckerBoard.perfomSoliderAction(i_SquareToMoveInNewBoard);
            o_CopyOfCheckerBoard.setParamatersForNextTurn();  
        }
    }
}
