using System;
using System.Collections.Generic;

namespace Program
{
    class CheckerBoard
    {
        private Player m_currentPlayer;
        private Player m_otherPlayer;
        private eSizeBoard m_sizeOfBoard;
        private eGameEndChoice m_gameEndChoice;
        private eGameStatus m_gameStatus;
        private MovementOptions m_movment;
        private Soldier m_soliderThatNeedToEatNextTurn;

      


        public CheckerBoard(Player i_FirstPlayer, Player i_SecondPlayer, eSizeBoard i_SizeOfBoard)
        {
            m_currentPlayer = i_FirstPlayer;
            m_otherPlayer = i_SecondPlayer;
            m_sizeOfBoard = i_SizeOfBoard;
            m_gameEndChoice = eGameEndChoice.Continue;
            m_gameStatus = eGameStatus.ContinueGame;
            m_movment = new MovementOptions(i_SizeOfBoard);
          
        }
        public void startGame()
        {

            while (m_gameEndChoice == eGameEndChoice.Continue)
            {

                while (m_gameStatus == eGameStatus.ContinueGame)
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    UIUtilities.PrintBoard(m_currentPlayer, m_otherPlayer, (int)m_sizeOfBoard);
                    nextTurn();
                    setParamatersForNextTurn();
                }
                caclculateResultGame();
                UIUtilities.printResultOnScreen(m_currentPlayer, m_otherPlayer, (int)m_sizeOfBoard);
                m_gameEndChoice = UIUtilities.getChoiseToContinuteTheGameFromClient();
                if (m_gameEndChoice == eGameEndChoice.Continue)
                {
                    initializeCheckerGame();
                }
            }
        }
        private void caclculateResultGame()
        {
            Player firstPlayer = getPlayer(eNumberOfPlayer.First);
            Player secondPlayer = getPlayer(eNumberOfPlayer.Second);
            switch (m_gameStatus)
            {
                case eGameStatus.FirstPlayerWon:
                    {
                        calculateAndSetPoints(firstPlayer, secondPlayer);
                        break;
                    }
                case eGameStatus.SecondPlayerWon:
                    {
                        calculateAndSetPoints(secondPlayer, firstPlayer);
                        break;
                    }
            }
        }
        private void setParamatersForNextTurn()
        {
            if (m_soliderThatNeedToEatNextTurn==null)
            {
                swapPlayers();
            }

        }
        private void nextTurn()
        {
            List<SquareMove> mustToDoMoves = new List<SquareMove>();
            List<SquareMove> availableVaildMoves = generateValidMovesOfPlayer(m_currentPlayer);
            if (!checkValidMove(availableVaildMoves))
            {
                determineResultGame();
            }
            else
            {
                
                initializeForMustMoves(availableVaildMoves, ref mustToDoMoves);
                SquareMove playerChoise = generateSquareToMove(availableVaildMoves, mustToDoMoves);
                if (playerChoise == null)
                {
                    m_gameStatus = eGameStatus.QExit;
                }
                else
                {
                    perfomSoliderAction(playerChoise);
                }
            }

        }
        private SquareMove generateSquareToMove(List<SquareMove> i_AvailableVaildMoves, List<SquareMove> i_MustToDoMoves)
        {
            SquareMove playerChoise;
            if (m_currentPlayer.TypeOfPlayer == eTypeOfPlayer.Human)
            {
                playerChoise = generateSquareToMoveHuman(m_currentPlayer, m_sizeOfBoard, i_AvailableVaildMoves, i_MustToDoMoves);
            }
            else
            {
                playerChoise = generateSquareToMoveComputer(i_AvailableVaildMoves, i_MustToDoMoves);
            }
            return playerChoise;
        }
        private SquareMove generateSquareToMoveComputer(List<SquareMove> i_AvaiableVaildMoves, List<SquareMove> i_MustToDoMoves)
        {
            SquareMove randomSquareMove;
            Random rnd = new Random();
            int randomIndex;
            if (i_MustToDoMoves.Count > 0)
            {
                randomIndex = rnd.Next(i_MustToDoMoves.Count);
                randomSquareMove = i_MustToDoMoves[randomIndex];
            }
            else
            {
                randomIndex = rnd.Next(i_AvaiableVaildMoves.Count);
                randomSquareMove = i_AvaiableVaildMoves[randomIndex];
            }
            return randomSquareMove;
        }
        private SquareMove generateSquareToMoveHuman(Player i_CurrentPlayer, eSizeBoard i_SizeOfBoard, List<SquareMove> i_AvaiableVaildMoves, List<SquareMove> i_MustToDoMoves)
        {
            SquareMove moveFromClient = null;
            bool isValidMove = false;
            while (!isValidMove)
            {
                moveFromClient=UIUtilities.getValidSquareToMoveFromClient(i_CurrentPlayer, i_SizeOfBoard);
                if (moveFromClient==null)
                {
                    break;
                }
                if (i_MustToDoMoves.Count > 0)
                {
                    isValidMove = i_MustToDoMoves.Contains(moveFromClient);
                }
                else
                {
                    isValidMove = i_AvaiableVaildMoves.Contains(moveFromClient);
                }
            }
            return moveFromClient;
        }
        private void initializeForMustMoves(List<SquareMove> i_AvaiableVaildMoves, ref List<SquareMove> i_MustToDoMoves)
        {
            if (m_soliderThatNeedToEatNextTurn==null)
            {
                addMustDoMoves(i_AvaiableVaildMoves, ref i_MustToDoMoves);
            }
            else
            {
                i_AvaiableVaildMoves = getValidMoveOfSolider(m_soliderThatNeedToEatNextTurn);
                addMustDoMoves(i_AvaiableVaildMoves, ref i_MustToDoMoves);
            }
        }
        private void addMustDoMoves(List<SquareMove> i_AvaiableVaildMoves, ref List<SquareMove> i_MustToDoMoves)
        {
            foreach (SquareMove currentMove in i_AvaiableVaildMoves)
            {
                if (currentMove.MustDoMove)
                {
                    i_MustToDoMoves.Add(currentMove);
                }
            }
        }
        private bool checkValidMove(List<SquareMove> i_AvaiableVaildMoves)
        {
            return i_AvaiableVaildMoves.Count > 0;
        }
        private List<SquareMove> generateValidMovesOfPlayer(Player i_Player)
        {
            List<SquareMove> validMoves = new List<SquareMove>();
            foreach (Soldier currentSoldier in i_Player.Soldiers)
            {
                validMoves.AddRange(getValidMoveOfSolider(currentSoldier));
            }
            return validMoves;

        }
        private List<SquareMove> getValidMoveOfSolider(Soldier i_Soldier)
        {
            List<SquareMove> validMoves = new List<SquareMove>();
            switch (i_Soldier.CharRepresent)
            {
                case Soldier.k_SecondPlayerRegular:
                    {
                        validMoves.AddRange(getValidMovesOfCurrentSoldierUpOrDown(i_Soldier, m_movment.MoveUp));
                        break;
                    }
                case Soldier.k_FirstPlayerRegular:
                    {
                        validMoves.AddRange(getValidMovesOfCurrentSoldierUpOrDown(i_Soldier, m_movment.MoveDown));
                        break;
                    }
                case Soldier.k_FirstPlayerKing:
                case Soldier.k_SecondPlayerKing:
                    {
                        validMoves.AddRange(getValidMovesOfCurrentSoldierUpOrDown(i_Soldier, m_movment.MoveDown));
                        validMoves.AddRange(getValidMovesOfCurrentSoldierUpOrDown(i_Soldier, m_movment.MoveUp));
                        break;
                    }

            }
            return validMoves;
        }
        private char whoIsInSquare(Square i_SquareToCheck)
        {
            char charRepresentPlayer = Soldier.k_emptySolider;
            List<Soldier> unifiedSoliderList = new List<Soldier>();
            unifiedSoliderList.AddRange(m_currentPlayer.Soldiers);
            unifiedSoliderList.AddRange(m_otherPlayer.Soldiers);
            foreach (Soldier tempSolider in unifiedSoliderList)
            {
                if (tempSolider.PlaceOnBoard.Equals(i_SquareToCheck))
                {
                    charRepresentPlayer = tempSolider.CharRepresent;
                    break;
                }
            }
            return charRepresentPlayer;
        }
        private SquareMove getVaildMoveFromSpesificSide(Soldier i_CurrentSolider, int i_RowMoveUpOrDown, int i_ColMoveRightOrLeft)
        {
            char? kingOfCurrentPlayer = null;
            char? regularOfCurrentPlayer = null;
            switch (m_currentPlayer.NumberOfPlayer)
            {
                case eNumberOfPlayer.First:
                    {
                        kingOfCurrentPlayer = Soldier.k_FirstPlayerKing;
                        regularOfCurrentPlayer = Soldier.k_FirstPlayerRegular;
                        break;
                    }
                case eNumberOfPlayer.Second:
                    {
                        kingOfCurrentPlayer = Soldier.k_SecondPlayerKing;
                        regularOfCurrentPlayer = Soldier.k_SecondPlayerRegular;
                        break;
                    }
            }
            SquareMove returnFinalSquareToMove = null;
            Square squareToMove = new Square((char)(i_CurrentSolider.PlaceOnBoard.Row + i_RowMoveUpOrDown), (char)(i_CurrentSolider.PlaceOnBoard.Col + i_ColMoveRightOrLeft));
            char soliderCharOfSquare = whoIsInSquare(squareToMove);
            //If the square is empty-> move to this square
            if (soliderCharOfSquare == Soldier.k_emptySolider)
            {
                returnFinalSquareToMove = new SquareMove(i_CurrentSolider.PlaceOnBoard, squareToMove);
            }
            //If the square is occupied and have the other player solider -> check if he can eat
            else if (soliderCharOfSquare != kingOfCurrentPlayer && soliderCharOfSquare != regularOfCurrentPlayer)
            {
                if (i_RowMoveUpOrDown == m_movment.MoveDown && i_CurrentSolider.PlaceOnBoard.Row < m_movment.EndRow-1 || i_RowMoveUpOrDown == m_movment.MoveUp && i_CurrentSolider.PlaceOnBoard.Row > MovementOptions.k_StartRow + 1)
                {
                    if ((i_ColMoveRightOrLeft == m_movment.MoveLeft && i_CurrentSolider.PlaceOnBoard.Col > MovementOptions.k_StartCol + 1) || (i_ColMoveRightOrLeft == m_movment.MoveRight && i_CurrentSolider.PlaceOnBoard.Col < m_movment.EndCol-1))
                    {
                        squareToMove = new Square((char)(i_CurrentSolider.PlaceOnBoard.Row + i_RowMoveUpOrDown * 2), (char)(i_CurrentSolider.PlaceOnBoard.Col + i_ColMoveRightOrLeft * 2));
                        soliderCharOfSquare = whoIsInSquare(squareToMove);
                        if (soliderCharOfSquare == Soldier.k_emptySolider)
                        {
                            returnFinalSquareToMove = new SquareMove((i_CurrentSolider.PlaceOnBoard), (squareToMove), true);
                        }
                    }
                }
            }
            return returnFinalSquareToMove;
        }
        private List<SquareMove> getValidMovesOfCurrentSoldierUpOrDown(Soldier i_CurrentSolider, int i_RowMoveUpOrDown)
        {
            List<SquareMove> tempVaildMoves = new List<SquareMove>();
            SquareMove tempMoveRight;
            SquareMove tempMoveLeft;
            if (i_RowMoveUpOrDown == m_movment.MoveDown && i_CurrentSolider.PlaceOnBoard.Row < m_movment.EndRow || i_RowMoveUpOrDown == m_movment.MoveUp && i_CurrentSolider.PlaceOnBoard.Row >MovementOptions.k_StartCol)
            {
                if (i_CurrentSolider.PlaceOnBoard.Col < m_movment.EndCol)
                {
                    tempMoveRight = getVaildMoveFromSpesificSide(i_CurrentSolider, i_RowMoveUpOrDown,m_movment.MoveRight);
                    if (tempMoveRight != null)
                    {
                        tempVaildMoves.Add(tempMoveRight);
                    }
                }
                
                if (i_CurrentSolider.PlaceOnBoard.Col > MovementOptions.k_StartCol)
                {
                    tempMoveLeft = getVaildMoveFromSpesificSide(i_CurrentSolider, i_RowMoveUpOrDown, m_movment.MoveLeft);
                    if (tempMoveLeft != null)
                    {
                        tempVaildMoves.Add(tempMoveLeft);
                    }
                }
            }
            return tempVaildMoves;
        }
        private void determineResultGame()
        {
            List<SquareMove> avaiableVaildMoves = generateValidMovesOfPlayer(m_otherPlayer);
            bool otherPlayerHasValidMove = checkValidMove(avaiableVaildMoves);
            if (otherPlayerHasValidMove)
            {
                setGameStatus(m_otherPlayer);
            }
            else
            {
                setGameStatus();
            }
        }
        private void swapPlayers()
        { 
            Player tempPlayer = m_currentPlayer;
            m_currentPlayer = m_otherPlayer;
            m_otherPlayer = tempPlayer;
        }
        private void perfomSoliderAction(SquareMove i_PlayerChoise)
        {
            foreach (Soldier currentSoldier in m_currentPlayer.Soldiers)
            {
                if (currentSoldier.PlaceOnBoard.Equals(i_PlayerChoise.FromSquare))
                {
                    currentSoldier.PlaceOnBoard = i_PlayerChoise.ToSquare;
                    UIUtilities.setCurrentMove(m_currentPlayer.PlayerName, currentSoldier.CharRepresent, i_PlayerChoise);
                    checkAndSetKingSolider(currentSoldier);
                    m_soliderThatNeedToEatNextTurn = null;
                    break;
                }
            }
            if ((Math.Abs(i_PlayerChoise.ToSquare.Col - i_PlayerChoise.FromSquare.Col)) == 2)
            {
                removeOtherPlayerSoliderFromBoard(i_PlayerChoise);
                setParamatersIfIsSoliderNeedToEatNextTurn(i_PlayerChoise.ToSquare);
            }
            if (m_otherPlayer.Soldiers.Count == 0)
            {
                setGameStatus(m_currentPlayer);
            }

        }
        private void checkAndSetKingSolider(Soldier currentSoldier)
        {
            if (currentSoldier.CharRepresent == Soldier.k_SecondPlayerRegular)
            {
                if (currentSoldier.PlaceOnBoard.Row == MovementOptions.k_StartRow)
                {
                    currentSoldier.CharRepresent = Soldier.k_SecondPlayerKing;
                    currentSoldier.TypeOfSoldier = eSoldierType.King;
                }
            }
            else if (currentSoldier.CharRepresent == Soldier.k_FirstPlayerRegular)
            {
                if (currentSoldier.PlaceOnBoard.Row == m_movment.EndRow)
                {
                    currentSoldier.CharRepresent = Soldier.k_FirstPlayerKing;
                    currentSoldier.TypeOfSoldier = eSoldierType.King;
                }
            }
        }
        private void setParamatersIfIsSoliderNeedToEatNextTurn(Square i_Square)
        {
            m_soliderThatNeedToEatNextTurn = null;
            List<SquareMove> validMoves = new List<SquareMove>();
            List<SquareMove> mustToDoMoves = new List<SquareMove>();
            foreach (Soldier currentSolider in m_currentPlayer.Soldiers)
            {
                if (currentSolider.PlaceOnBoard.Equals(i_Square))
                {
                    validMoves = getValidMoveOfSolider(currentSolider);
                    initializeForMustMoves(validMoves, ref mustToDoMoves);
                    if (mustToDoMoves.Count > 0)
                    {
                        m_soliderThatNeedToEatNextTurn = currentSolider;
                    }
                    break;
                }
            }

        }
        private void removeOtherPlayerSoliderFromBoard(SquareMove i_PlayerChoise)
        {

            char rowOfOtherPlayerToRemove = i_PlayerChoise.FromSquare.Row;
            char colOfOtherPlayerToRemove = i_PlayerChoise.FromSquare.Col;
            calculateSquareotherPlayerToRemove(i_PlayerChoise.ToSquare.Row, i_PlayerChoise.FromSquare.Row, ref rowOfOtherPlayerToRemove);
            calculateSquareotherPlayerToRemove(i_PlayerChoise.ToSquare.Col, i_PlayerChoise.FromSquare.Col, ref colOfOtherPlayerToRemove);
            m_otherPlayer.RemoveSolider(new Square(rowOfOtherPlayerToRemove, colOfOtherPlayerToRemove));
        }
        private void calculateSquareotherPlayerToRemove(char i_ToSquare, char i_FromSquare, ref char i_SquareToCalculate)
        {
            if ((i_ToSquare - (i_FromSquare)) > 0)
            {
                i_SquareToCalculate += (char)1;
            }
            else
            {
                i_SquareToCalculate -= (char)1;
            }
        }
        private void setGameStatus(Player i_WineerPlayer = null)
        {
            if (i_WineerPlayer == null)
            {
                m_gameStatus = eGameStatus.Tie;
            }
            else
            {
                if (i_WineerPlayer.NumberOfPlayer == eNumberOfPlayer.First)
                {
                    m_gameStatus = eGameStatus.FirstPlayerWon;
                }
                else
                {
                    m_gameStatus = eGameStatus.SecondPlayerWon;
                }
            }
        }
        private void calculateAndSetPoints(Player i_Winner, Player i_Loser)
        {
            int resultOfPoints = i_Winner.calculatePointsOfSoliders() - i_Loser.calculatePointsOfSoliders();
            i_Winner.Score = i_Winner.Score + resultOfPoints;
        }
        private Player getPlayer(eNumberOfPlayer i_NumberOfPlayer)
        {
            Player playerToReturn;
            if (m_currentPlayer.NumberOfPlayer == i_NumberOfPlayer)
            {
                playerToReturn = m_currentPlayer;
            }
            else
            {
                playerToReturn = m_otherPlayer;
            }
            return playerToReturn;

        }
        private void initializeCheckerGame()
        {
            Player firstPlayer = getPlayer(eNumberOfPlayer.First);
            Player secondPlayer = getPlayer(eNumberOfPlayer.Second);
            firstPlayer.generateSoliders(eNumberOfPlayer.First, m_sizeOfBoard);
            secondPlayer.generateSoliders(eNumberOfPlayer.Second, m_sizeOfBoard);
            m_currentPlayer = firstPlayer;
            m_otherPlayer = secondPlayer;
            m_gameStatus = eGameStatus.ContinueGame;
         
            m_soliderThatNeedToEatNextTurn = null;
            UIUtilities.initializeParameters();
        }
    }
}
