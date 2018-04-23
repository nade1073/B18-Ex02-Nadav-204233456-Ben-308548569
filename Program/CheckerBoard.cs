namespace Program
{
    using System;
    using System.Collections.Generic;

    public class CheckerBoard
    {
        private eSizeBoard r_SizeOfBoard;
        private Player m_CurrentPlayer;
        private Player m_OtherPlayer;
        private eGameEndChoice m_GameEndChoice=eGameEndChoice.Continue;
        private eGameStatus m_GameStatus= eGameStatus.ContinueGame;
        private MovementOptions m_MovmentOption;

        private Soldier m_SoliderThatNeedToEatNextTurn;

        public CheckerBoard()
        {
        }

        public CheckerBoard(CheckerBoard i_CloneToThisBoard)
        {
            Player otherFirstPlayer = i_CloneToThisBoard.m_CurrentPlayer;
            Player otherSecondPlayer = i_CloneToThisBoard.m_OtherPlayer;
            m_CurrentPlayer = new Player(otherFirstPlayer.PlayerName, otherFirstPlayer.TypeOfPlayer, otherFirstPlayer.NumberOfPlayer, i_CloneToThisBoard.r_SizeOfBoard);
            m_OtherPlayer = new Player(otherSecondPlayer.PlayerName, otherSecondPlayer.TypeOfPlayer, otherSecondPlayer.NumberOfPlayer, i_CloneToThisBoard.r_SizeOfBoard);
            r_SizeOfBoard = i_CloneToThisBoard.r_SizeOfBoard;
            m_GameEndChoice = i_CloneToThisBoard.m_GameEndChoice;
            m_GameStatus = i_CloneToThisBoard.m_GameStatus;
            m_SoliderThatNeedToEatNextTurn = null;
            m_MovmentOption = i_CloneToThisBoard.m_MovmentOption;
            m_CurrentPlayer.Soldiers = new List<Soldier>();
            m_OtherPlayer.Soldiers = new List<Soldier>();
            if (i_CloneToThisBoard.m_SoliderThatNeedToEatNextTurn != null)
            {
                m_SoliderThatNeedToEatNextTurn = new Soldier(i_CloneToThisBoard.m_SoliderThatNeedToEatNextTurn.CharRepresent, i_CloneToThisBoard.m_SoliderThatNeedToEatNextTurn.PlaceOnBoard, i_CloneToThisBoard.m_SoliderThatNeedToEatNextTurn.TypeOfSoldier);
            }
            
            foreach (Soldier currentSolider in i_CloneToThisBoard.m_CurrentPlayer.Soldiers)
            {
                m_CurrentPlayer.Soldiers.Add(new Soldier(currentSolider.CharRepresent, currentSolider.PlaceOnBoard, currentSolider.TypeOfSoldier));
            }
            foreach (Soldier currentSolider in i_CloneToThisBoard.m_OtherPlayer.Soldiers)
            {
                m_OtherPlayer.Soldiers.Add(new Soldier(currentSolider.CharRepresent, currentSolider.PlaceOnBoard, currentSolider.TypeOfSoldier));
            }
        }

        public void startGame()
        {
            initializeStartCheckerBoard();
            while (m_GameEndChoice == eGameEndChoice.Continue)
            {
                while (m_GameStatus == eGameStatus.ContinueGame)
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    UIUtilities.PrintBoard(m_CurrentPlayer, m_OtherPlayer, (int)r_SizeOfBoard);
                    nextTurn();
                    setParamatersForNextTurn();
                }

                caclculateResultGame();
                UIUtilities.printResultOnScreen(m_CurrentPlayer, m_OtherPlayer, (int)r_SizeOfBoard);
                m_GameEndChoice = UIUtilities.getChoiseToContinuteTheGameFromClient();
                if (m_GameEndChoice == eGameEndChoice.Continue)
                {
                    initializeCheckerGame();
                }
            }
        }

        private void initializeStartCheckerBoard()
        {
            String firstPlayerName,secondPlayerName;
            eSizeBoard sizeOfBoard;
            UIUtilities.getClientNamesAndTypeOfSecondPlayer(out firstPlayerName, out secondPlayerName, out sizeOfBoard);
            m_CurrentPlayer = new Player(firstPlayerName, eTypeOfPlayer.Human, eNumberOfPlayer.First, sizeOfBoard);
            r_SizeOfBoard = sizeOfBoard;
            if (secondPlayerName == null)
            {
                m_OtherPlayer = new Player(Player.k_computerName, eTypeOfPlayer.Computer, eNumberOfPlayer.Second, sizeOfBoard);
            }
            else
            {
                m_OtherPlayer = new Player(secondPlayerName, eTypeOfPlayer.Human, eNumberOfPlayer.Second, sizeOfBoard);
            }
            m_MovmentOption = new MovementOptions(r_SizeOfBoard);
        }

        private void caclculateResultGame()
        {
            Player firstPlayer = getPlayer(eNumberOfPlayer.First);
            Player secondPlayer = getPlayer(eNumberOfPlayer.Second);
            switch (m_GameStatus)
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
                case eGameStatus.QExit:
                    {
                        m_CurrentPlayer.Score += 4;
                        break;
                    }
            }
        }

        private void setParamatersForNextTurn()
        {
            if (m_SoliderThatNeedToEatNextTurn == null)
            {
                swapPlayers();
            }
        }

        private void nextTurn()
        {
            List<SquareMove> mustToDoMoves = new List<SquareMove>();
            List<SquareMove> availableVaildMoves = generateValidMovesOfPlayer(m_CurrentPlayer);
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
                    m_GameStatus = eGameStatus.QExit;
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
            if (m_CurrentPlayer.TypeOfPlayer == eTypeOfPlayer.Human)
            {
                playerChoise = generateSquareToMoveHuman(m_CurrentPlayer, r_SizeOfBoard, i_AvailableVaildMoves, i_MustToDoMoves);
            }
            else
            {
                playerChoise = generateSquareToMoveComputer(i_AvailableVaildMoves, i_MustToDoMoves);
            }

            return playerChoise;
        }

        private SquareMove generateSquareToMoveComputer(List<SquareMove> i_AvaiableVaildMoves, List<SquareMove> i_MustToDoMoves)
        {
            IAChecker chekcerCloneToCurrentBoard = new IAChecker(this);
            List<AIMovementScore> avalibaleMovmenetsToCalculate = new List<AIMovementScore>();
            if (i_MustToDoMoves.Count > 0)
            {
                foreach(SquareMove currentMove in i_MustToDoMoves)
                {
                    avalibaleMovmenetsToCalculate.Add(new AIMovementScore(currentMove));
                }
            }
            else
            {
                foreach (SquareMove currentMove in i_AvaiableVaildMoves)
                {
                    avalibaleMovmenetsToCalculate.Add(new AIMovementScore(currentMove));
                }
            }

            return chekcerCloneToCurrentBoard.IACheckerCalculateNextMove(avalibaleMovmenetsToCalculate);
        }

        private SquareMove generateSquareToMoveHuman(Player i_CurrentPlayer, eSizeBoard i_SizeOfBoard, List<SquareMove> i_AvaiableVaildMoves, List<SquareMove> i_MustToDoMoves)
        {
            SquareMove moveFromClient = null;
            bool isValidMove = false;
            while (!isValidMove)
            {
                moveFromClient = UIUtilities.getValidSquareToMoveFromClient(i_CurrentPlayer, i_SizeOfBoard);
                if (moveFromClient == null)
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

        private void initializeForMustMoves(List<SquareMove> i_AvaiableVaildMoves, ref List<SquareMove> io_MustToDoMoves)
        {
            if (m_SoliderThatNeedToEatNextTurn == null)
            {
                addMustDoMoves(i_AvaiableVaildMoves, ref io_MustToDoMoves);
            }
            else
            {
                i_AvaiableVaildMoves = getValidMoveOfSolider(m_SoliderThatNeedToEatNextTurn);
                addMustDoMoves(i_AvaiableVaildMoves, ref io_MustToDoMoves);
            }
        }
         
        private void addMustDoMoves(List<SquareMove> i_AvaiableVaildMoves, ref List<SquareMove> io_MustToDoMoves)
        {
            foreach (SquareMove currentMove in i_AvaiableVaildMoves)
            {
                if (currentMove.MustDoMove)
                {
                    io_MustToDoMoves.Add(currentMove);
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
                        validMoves.AddRange(getValidMovesOfCurrentSoldierUpOrDown(i_Soldier, m_MovmentOption.MoveUp));
                        break;
                    }

                case Soldier.k_FirstPlayerRegular:
                    {
                        validMoves.AddRange(getValidMovesOfCurrentSoldierUpOrDown(i_Soldier, m_MovmentOption.MoveDown));
                        break;
                    }

                case Soldier.k_FirstPlayerKing:
                case Soldier.k_SecondPlayerKing:
                    {
                        validMoves.AddRange(getValidMovesOfCurrentSoldierUpOrDown(i_Soldier, m_MovmentOption.MoveDown));
                        validMoves.AddRange(getValidMovesOfCurrentSoldierUpOrDown(i_Soldier, m_MovmentOption.MoveUp));
                        break;
                    }
            }

            return validMoves;
        }

        private char whoIsInSquare(Square i_SquareToCheck)
        {
            char charRepresentPlayer = Soldier.k_EmptySolider;
            List<Soldier> unifiedSoliderList = new List<Soldier>();
            unifiedSoliderList.AddRange(m_CurrentPlayer.Soldiers);
            unifiedSoliderList.AddRange(m_OtherPlayer.Soldiers);
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
            switch (m_CurrentPlayer.NumberOfPlayer)
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
            ////If the square is empty-> move to this square
            ////Else if the square is occupied and have the other player solider -> check if he can eat
            if (soliderCharOfSquare == Soldier.k_EmptySolider)
            {
                returnFinalSquareToMove = new SquareMove(i_CurrentSolider.PlaceOnBoard, squareToMove);
            }           
            else if (soliderCharOfSquare != kingOfCurrentPlayer && soliderCharOfSquare != regularOfCurrentPlayer)
            {
                if ((i_RowMoveUpOrDown == m_MovmentOption.MoveDown && i_CurrentSolider.PlaceOnBoard.Row < m_MovmentOption.EndRow - 1) || (i_RowMoveUpOrDown == m_MovmentOption.MoveUp && i_CurrentSolider.PlaceOnBoard.Row > MovementOptions.k_StartRow + 1))
                {
                    if ((i_ColMoveRightOrLeft == m_MovmentOption.MoveLeft && i_CurrentSolider.PlaceOnBoard.Col > MovementOptions.k_StartCol + 1) || (i_ColMoveRightOrLeft == m_MovmentOption.MoveRight && i_CurrentSolider.PlaceOnBoard.Col < m_MovmentOption.EndCol - 1))
                    {
                        squareToMove = new Square((char)(i_CurrentSolider.PlaceOnBoard.Row + (i_RowMoveUpOrDown * 2)), (char)(i_CurrentSolider.PlaceOnBoard.Col + (i_ColMoveRightOrLeft * 2)));
                        soliderCharOfSquare = whoIsInSquare(squareToMove);
                        if (soliderCharOfSquare == Soldier.k_EmptySolider)
                        {
                            returnFinalSquareToMove = new SquareMove(i_CurrentSolider.PlaceOnBoard, squareToMove, true);
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
            if ((i_RowMoveUpOrDown == m_MovmentOption.MoveDown && i_CurrentSolider.PlaceOnBoard.Row < m_MovmentOption.EndRow) || (i_RowMoveUpOrDown == m_MovmentOption.MoveUp && i_CurrentSolider.PlaceOnBoard.Row > MovementOptions.k_StartRow))
            {
                if (i_CurrentSolider.PlaceOnBoard.Col < m_MovmentOption.EndCol)
                {
                    tempMoveRight = getVaildMoveFromSpesificSide(i_CurrentSolider, i_RowMoveUpOrDown, m_MovmentOption.MoveRight);
                    if (tempMoveRight != null)
                    {
                        tempVaildMoves.Add(tempMoveRight);
                    }
                }
                
                if (i_CurrentSolider.PlaceOnBoard.Col > MovementOptions.k_StartCol)
                {
                    tempMoveLeft = getVaildMoveFromSpesificSide(i_CurrentSolider, i_RowMoveUpOrDown, m_MovmentOption.MoveLeft);
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
            List<SquareMove> avaiableVaildMoves = generateValidMovesOfPlayer(m_OtherPlayer);
            bool otherPlayerHasValidMove = checkValidMove(avaiableVaildMoves);
            if (otherPlayerHasValidMove)
            {
                setGameStatus(m_OtherPlayer);
            }
            else
            {
                setGameStatus();
            }
        }

        private void swapPlayers()
        { 
            Player tempPlayer = m_CurrentPlayer;
            m_CurrentPlayer = m_OtherPlayer;
            m_OtherPlayer = tempPlayer;
        }
       
        private void perfomSoliderAction(SquareMove i_PlayerChoise)
        {
            foreach (Soldier currentSoldier in m_CurrentPlayer.Soldiers)
            {
                if (currentSoldier.PlaceOnBoard.Equals(i_PlayerChoise.FromSquare))
                {
                    currentSoldier.PlaceOnBoard = i_PlayerChoise.ToSquare;
                    UIUtilities.setCurrentMove(m_CurrentPlayer.PlayerName, currentSoldier.CharRepresent, i_PlayerChoise);
                    checkAndSetKingSolider(currentSoldier);
                    m_SoliderThatNeedToEatNextTurn = null;
                    break;
                }
            }

            if (Math.Abs(i_PlayerChoise.ToSquare.Col - i_PlayerChoise.FromSquare.Col) == 2)
            {
                removeOtherPlayerSoliderFromBoard(i_PlayerChoise);
                setParamatersIfIsSoliderNeedToEatNextTurn(i_PlayerChoise.ToSquare);
            }

            if (m_OtherPlayer.Soldiers.Count == 0)
            {
                setGameStatus(m_CurrentPlayer);
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
                if (currentSoldier.PlaceOnBoard.Row == m_MovmentOption.EndRow)
                {
                    currentSoldier.CharRepresent = Soldier.k_FirstPlayerKing;
                    currentSoldier.TypeOfSoldier = eSoldierType.King;
                }
            }
        }

        private void setParamatersIfIsSoliderNeedToEatNextTurn(Square i_Square)
        {
            m_SoliderThatNeedToEatNextTurn = null;
            List<SquareMove> validMoves = new List<SquareMove>();
            List<SquareMove> mustToDoMoves = new List<SquareMove>();
            foreach (Soldier currentSolider in m_CurrentPlayer.Soldiers)
            {
                if (currentSolider.PlaceOnBoard.Equals(i_Square))
                {
                    validMoves = getValidMoveOfSolider(currentSolider);
                    initializeForMustMoves(validMoves, ref mustToDoMoves);
                    if (mustToDoMoves.Count > 0)
                    {
                        m_SoliderThatNeedToEatNextTurn = currentSolider;
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
            m_OtherPlayer.RemoveSolider(new Square(rowOfOtherPlayerToRemove, colOfOtherPlayerToRemove));
            //--Added forIA
            m_CurrentPlayer.RemoveSolider(new Square(rowOfOtherPlayerToRemove, colOfOtherPlayerToRemove));
        }

        private void calculateSquareotherPlayerToRemove(char i_ToSquare, char i_FromSquare, ref char io_SquareToCalculate)
        {
            if (i_ToSquare - i_FromSquare > 0)
            {
                io_SquareToCalculate += (char)1;
            }
            else
            {
                io_SquareToCalculate -= (char)1;
            }
        }

        private void setGameStatus(Player i_WineerPlayer = null)
        {
            if (i_WineerPlayer == null)
            {
                m_GameStatus = eGameStatus.Tie;
            }
            else
            {
                if (i_WineerPlayer.NumberOfPlayer == eNumberOfPlayer.First)
                {
                    m_GameStatus = eGameStatus.FirstPlayerWon;
                }
                else
                {
                    m_GameStatus = eGameStatus.SecondPlayerWon;
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
            if (m_CurrentPlayer.NumberOfPlayer == i_NumberOfPlayer)
            {
                playerToReturn = m_CurrentPlayer;
            }
            else
            {
                playerToReturn = m_OtherPlayer;
            }

            return playerToReturn;
        }

        private void initializeCheckerGame()
        {
            Player firstPlayer = getPlayer(eNumberOfPlayer.First);
            Player secondPlayer = getPlayer(eNumberOfPlayer.Second);
            firstPlayer.generateSoliders(eNumberOfPlayer.First, r_SizeOfBoard);
            secondPlayer.generateSoliders(eNumberOfPlayer.Second, r_SizeOfBoard);
            m_CurrentPlayer = firstPlayer;
            m_OtherPlayer = secondPlayer;
            m_GameStatus = eGameStatus.ContinueGame;
            m_SoliderThatNeedToEatNextTurn = null;
            UIUtilities.initializeParameters();
        }

        private class IAChecker
        {
            private CheckerBoard m_TempCloneBoard;

            private BoardSquareScore m_ScoresOfBoard=null;
            public const int k_IADepth = 5;
            public IAChecker(CheckerBoard i_CheckerBoard)
            {
                initializeClassMembers(i_CheckerBoard);
                
            }
            private void initializeClassMembers(CheckerBoard i_CheckerBoard)
            {
                m_TempCloneBoard = new CheckerBoard(i_CheckerBoard);
                if (m_ScoresOfBoard == null)
                {
                    m_ScoresOfBoard = new BoardSquareScore(i_CheckerBoard.r_SizeOfBoard);
                }
            }
            public SquareMove IACheckerCalculateNextMove(List<AIMovementScore> i_ListOfAllMovements)
            {
                double alpha = Double.NegativeInfinity;
                double beta = Double.PositiveInfinity;
                CheckerBoard tempBoard = null;
                Boolean maxmizingPlayer = true;
                foreach (AIMovementScore currentMove in i_ListOfAllMovements)
                {
                    
                    performMoveAndSwitchPlayers(m_TempCloneBoard, out tempBoard, currentMove.SquareMove);
                    currentMove.ScoreOfMove = miniMaxAlgorithem(tempBoard, IAChecker.k_IADepth, !maxmizingPlayer, alpha,beta);
                    currentMove.ScoreInBoard = m_ScoresOfBoard.ArrayOfScores[currentMove.ToSquare.Row - 'a',currentMove.ToSquare.Col-'A'];
                }
                double maxHeuristics = Double.NegativeInfinity;
                foreach (AIMovementScore currentMove in i_ListOfAllMovements)
                {
                    if(currentMove.ScoreOfMove > maxHeuristics)
                    {
                        maxHeuristics = currentMove.ScoreOfMove;
                    }
                }
                i_ListOfAllMovements.RemoveAll(item => item.ScoreOfMove < maxHeuristics);
                int maxOfScoreInBoard = 0;
                if(i_ListOfAllMovements.Count>0)
                {
                    foreach (AIMovementScore currentMove in i_ListOfAllMovements)
                    {
                        if (currentMove.ScoreInBoard > maxOfScoreInBoard)
                        {
                            maxOfScoreInBoard = currentMove.ScoreInBoard;
                        }
                    }
                }
                Random rand = new Random();
                i_ListOfAllMovements.RemoveAll(item => item.ScoreInBoard < maxOfScoreInBoard);
                int randomIndex = rand.Next(i_ListOfAllMovements.Count);
                return i_ListOfAllMovements[randomIndex].SquareMove;
            }
            private double getHeuristic(CheckerBoard i_Board)
            {
                double kingWeight = 1.3;
                double result = 0;

                if(i_Board.m_CurrentPlayer.TypeOfPlayer==eTypeOfPlayer.Computer)
                {
                    result = i_Board.m_CurrentPlayer.getNumberOfSpesificSoldierType(eSoldierType.King) * kingWeight + i_Board.m_CurrentPlayer.getNumberOfSpesificSoldierType(eSoldierType.Regular) - i_Board.m_OtherPlayer.getNumberOfSpesificSoldierType(eSoldierType.King) * kingWeight - i_Board.m_OtherPlayer.getNumberOfSpesificSoldierType(eSoldierType.Regular);
                }
                else
                {
                    result = i_Board.m_OtherPlayer.getNumberOfSpesificSoldierType(eSoldierType.King) * kingWeight + i_Board.m_OtherPlayer.getNumberOfSpesificSoldierType(eSoldierType.Regular) - i_Board.m_CurrentPlayer.getNumberOfSpesificSoldierType(eSoldierType.King) * kingWeight - i_Board.m_CurrentPlayer.getNumberOfSpesificSoldierType(eSoldierType.Regular);
                }
               
                return result;

            }
            private Double miniMaxAlgorithem(CheckerBoard i_Board, int i_Depth,bool i_MaxmizingPlayer, double i_Alpha, double i_Beta)
            {
                if (i_Depth == 0)
                {
                    return getHeuristic(i_Board);
                }
                List<SquareMove> availableVaildMoves;
                if (i_Board.m_SoliderThatNeedToEatNextTurn == null)
                {
                   availableVaildMoves = i_Board.generateValidMovesOfPlayer(i_Board.m_CurrentPlayer);
                }
                else
                {
                    availableVaildMoves = i_Board.getValidMoveOfSolider(i_Board.m_SoliderThatNeedToEatNextTurn);
                }
                double initial=0;
                CheckerBoard tempBoard = null;

                if(i_MaxmizingPlayer)
                {
                    initial = Double.NegativeInfinity;
                    foreach (SquareMove currentMove in availableVaildMoves)
                    {
                        double result = 0;
                        performMoveAndSwitchPlayers(i_Board, out tempBoard, currentMove);
                        if (tempBoard.m_SoliderThatNeedToEatNextTurn != null)
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
                        if (tempBoard.m_SoliderThatNeedToEatNextTurn != null)
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
            private void performMoveAndSwitchPlayers(CheckerBoard i_Original,out CheckerBoard o_CopyOfCheckerBoard,SquareMove i_SquareToMoveInNewBoard)
            {
                o_CopyOfCheckerBoard = new CheckerBoard(i_Original);
                o_CopyOfCheckerBoard.perfomSoliderAction(i_SquareToMoveInNewBoard);
                if (o_CopyOfCheckerBoard.m_SoliderThatNeedToEatNextTurn == null)
                {
                    Player tempPlayer = o_CopyOfCheckerBoard.m_CurrentPlayer;
                    o_CopyOfCheckerBoard.m_CurrentPlayer = o_CopyOfCheckerBoard.m_OtherPlayer;
                    o_CopyOfCheckerBoard.m_OtherPlayer = tempPlayer;
                }
            }
        }

    }
}
