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
        private String m_MovementStatement;
        private bool m_isSoliderNeedToEatNextTurn;
        private Soldier m_soliderThatNeedToEatNextTurn;




        public CheckerBoard(Player i_FirstPlayer,Player i_SecondPlayer,eSizeBoard i_SizeOfBoard)
        {
            m_currentPlayer = i_FirstPlayer;
            m_otherPlayer = i_SecondPlayer;
            m_sizeOfBoard = i_SizeOfBoard;
            m_gameEndChoice = eGameEndChoice.Continue;
            m_gameStatus = eGameStatus.ContinueGame;
            m_isSoliderNeedToEatNextTurn = false;
            m_soliderThatNeedToEatNextTurn = null;
            m_MovementStatement = null; 
        }
        public void startGame()
        {

            while(m_gameEndChoice==eGameEndChoice.Continue)
            {
                
                while(m_gameStatus==eGameStatus.ContinueGame)
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    UIUtilities.PrintBoard(m_currentPlayer, m_otherPlayer,(int)m_sizeOfBoard);
                    if (m_MovementStatement != null)
                    {
                        System.Console.WriteLine(m_MovementStatement);
                    }
                    nextTurn();
                    setParamatersForNextTurn();
                }
                caclculateResultGame();
                UIUtilities.printResultOnScreen(m_currentPlayer, m_otherPlayer);
                m_gameEndChoice = UIUtilities.getChoiseToContinuteTheGameFromClient();
                if(m_gameEndChoice==eGameEndChoice.Continue)
                {
                    initializeCheckerGame();
                }
            }      
        }
        private void caclculateResultGame()
        {
            Player firstPlayer = getPlayer(eNumberOfPlayer.First);
            Player secondplayer = getPlayer(eNumberOfPlayer.Second);
            switch(m_gameStatus)
            {
                case eGameStatus.FirstPlayerWon:
                    {
                        calculateAndSetPoints(firstPlayer, secondplayer);

                        break;
                    }
                case eGameStatus.SecondPlayerWon:
                    {
                        calculateAndSetPoints(secondplayer, firstPlayer);
                        break;
                    }
            }
        }
        private void setParamatersForNextTurn()
        {
        if(!m_isSoliderNeedToEatNextTurn)
            {
                swapPlayers();
            }

        }
        private void nextTurn()
        {
            List<SquareMove> mustToDoMoves = new List<SquareMove>();
            List<SquareMove> avaiableVaildMoves = generateValidMovesOfPlayer(m_currentPlayer);
            if (!checkValidMove(avaiableVaildMoves))
            {
                determineResultGame();
            }
            else
            {
                initializeForMustMoves(avaiableVaildMoves, ref mustToDoMoves);
                SquareMove playerChoise=UIUtilities.getValidSquareToMoveFromClient(m_currentPlayer, m_sizeOfBoard,avaiableVaildMoves,mustToDoMoves);
                if(playerChoise==null)
                {
                    m_gameStatus = eGameStatus.QExit;
                }
                else
                {
                    perfomSoliderAction(playerChoise);
                }
            }

        }
        private void initializeForMustMoves(List<SquareMove> i_AvaiableVaildMoves, ref List<SquareMove> i_MustToDoMoves)
        {
            if (!m_isSoliderNeedToEatNextTurn)
            {
                foreach (SquareMove currentMove in i_AvaiableVaildMoves)
                {
                    if (currentMove.GetMustDoMove())
                    {
                        i_MustToDoMoves.Add(currentMove);
                    }
                }
            }
            else
            {
                i_MustToDoMoves = getValidMoveOfSolider(m_soliderThatNeedToEatNextTurn);
            }
        }
        private bool checkValidMove(List<SquareMove> i_AvaiableVaildMoves)
        {
            return i_AvaiableVaildMoves.Capacity > 0;
        }
        private List<SquareMove> generateValidMovesOfPlayer(Player i_Player)
        {
            List<SquareMove> validMoves = new List<SquareMove>();
            List<Soldier> currentPlayerSoldiers = i_Player.GetSoldiers();
            foreach(Soldier currentSoldier in currentPlayerSoldiers)
            {
                validMoves.AddRange(getValidMoveOfSolider(currentSoldier));
            }
            return validMoves;

        }
        private List<SquareMove> getValidMoveOfSolider(Soldier currentSoldier)
        {
            List<SquareMove> validMoves = new List<SquareMove>();
            switch (currentSoldier.GetRepresentChar())
            {
                case 'X':
                    {
                        validMoves.AddRange(getValidMovesOfCurrentSoldierUpOrDown(currentSoldier, eUpOrDown.Up));
                        break;
                    }
                case 'O':
                    {
                        validMoves.AddRange(getValidMovesOfCurrentSoldierUpOrDown(currentSoldier, eUpOrDown.Down));
                        break;
                    }
                case 'U':
                case 'K':
                    {
                        validMoves.AddRange(getValidMovesOfCurrentSoldierUpOrDown(currentSoldier, eUpOrDown.Down));
                        validMoves.AddRange(getValidMovesOfCurrentSoldierUpOrDown(currentSoldier, eUpOrDown.Up));
                        break;
                    }

            }
            return validMoves;
        }
        private char whoIsInSquare(Square i_SquareToCheck)
        {
            char charRepresentPlayer = ' ';
            List<Soldier> unifiedSoliderList = new List<Soldier>();
            unifiedSoliderList.AddRange(m_currentPlayer.GetSoldiers());
            unifiedSoliderList.AddRange(m_otherPlayer.GetSoldiers());
            foreach (Soldier tempSolider in unifiedSoliderList)
            {
                if(tempSolider.GetPlaceOnBoard().Equals(i_SquareToCheck))
                {
                    charRepresentPlayer = tempSolider.GetRepresentChar();
                    break;
                }
            }

            return charRepresentPlayer;
            

        }
        private SquareMove getVaildMoveFromSpesificSide(Soldier i_CurrentSolider, eUpOrDown i_RowMove, eRightOrLeft i_ColMove)
        {
            char kingOfCurrentPlayer=' ';
            char regularOfCurrentPlayer= ' ';
            switch(m_currentPlayer.GetNumberOfPlayer())
            {
                case eNumberOfPlayer.First:
                    {
                        kingOfCurrentPlayer = 'U';
                        regularOfCurrentPlayer='O';
                        break;
                    }
                case eNumberOfPlayer.Second:
                    {
                        kingOfCurrentPlayer = 'K';
                        regularOfCurrentPlayer = 'X';
                        break;
                    }
            }
            SquareMove tempSquareToMove = null;  
            Square squareToMove = new Square((char)(i_CurrentSolider.GetPlaceOnBoard().GetRow() + i_RowMove), (char)(i_CurrentSolider.GetPlaceOnBoard().GetCol()+ i_ColMove));
            char soliderCharOfSquare = whoIsInSquare(squareToMove);

            if (soliderCharOfSquare == ' ')
            {
                tempSquareToMove = new SquareMove((i_CurrentSolider.GetPlaceOnBoard()), (squareToMove));
            }
            else if (soliderCharOfSquare != kingOfCurrentPlayer && soliderCharOfSquare!= regularOfCurrentPlayer)
            {
                if (i_RowMove == eUpOrDown.Down && i_CurrentSolider.GetPlaceOnBoard().GetRow() < ('a' + (int)m_sizeOfBoard - 2) || i_RowMove == eUpOrDown.Up && i_CurrentSolider.GetPlaceOnBoard().GetRow() > 'b')
                {
                    if((i_ColMove == eRightOrLeft.Left && i_CurrentSolider.GetPlaceOnBoard().GetCol() > 'B' )||(i_ColMove == eRightOrLeft.Right && i_CurrentSolider.GetPlaceOnBoard().GetCol() < ('A' + (int)m_sizeOfBoard - 2)))
                    {
                        squareToMove = new Square((char)(i_CurrentSolider.GetPlaceOnBoard().GetRow() +(int) i_RowMove*2), (char)(i_CurrentSolider.GetPlaceOnBoard().GetCol() +(int)i_ColMove * 2));
                        soliderCharOfSquare = whoIsInSquare(squareToMove);
                        if(soliderCharOfSquare==' ')
                        {
                            tempSquareToMove = new SquareMove((i_CurrentSolider.GetPlaceOnBoard()), (squareToMove),true);
                        }
                    }
                }
            }
            return tempSquareToMove;
        }
        private List<SquareMove> getValidMovesOfCurrentSoldierUpOrDown(Soldier i_CurrentSolider,eUpOrDown i_RowMove)
        {
            List<SquareMove> tempVaildMoves = new List<SquareMove>();
            SquareMove tempMoveRight;
            SquareMove tempMoveLeft;
            if (i_RowMove == eUpOrDown.Down && i_CurrentSolider.GetPlaceOnBoard().GetRow() < ('a'+(int)m_sizeOfBoard-1) || i_RowMove==eUpOrDown.Up && i_CurrentSolider.GetPlaceOnBoard().GetRow() > 'a' )
            {
                if (i_CurrentSolider.GetPlaceOnBoard().GetCol() < (((char)m_sizeOfBoard - 1) + 'A'))
                {
                    tempMoveRight = getVaildMoveFromSpesificSide(i_CurrentSolider, i_RowMove, eRightOrLeft.Right);
                    if (tempMoveRight != null)
                    {
                        tempVaildMoves.Add(tempMoveRight);
                    }
                }
                if (i_CurrentSolider.GetPlaceOnBoard().GetCol() != 'A')
                {
                    tempMoveLeft = getVaildMoveFromSpesificSide(i_CurrentSolider, i_RowMove, eRightOrLeft.Left);
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
            foreach(Soldier currentSoldier in m_currentPlayer.GetSoldiers())
            {
                if(currentSoldier.GetPlaceOnBoard().Equals(i_PlayerChoise.GetFromSquare()))
                {
                    currentSoldier.SetPlaceOnBoard(i_PlayerChoise.GetToSquare());
                     m_MovementStatement=UIUtilities.printCurrentMove(m_currentPlayer.GetPlayerName(), currentSoldier.GetRepresentChar(), i_PlayerChoise);
                    checkAndSetKingSolider(currentSoldier);
                    break;
                }
            }
            if((Math.Abs(i_PlayerChoise.GetToSquare().GetCol()-i_PlayerChoise.GetFromSquare().GetCol()))==2)
            {
                removeOtherPlayerSoliderFromBoard(i_PlayerChoise);
                setParamatersIfIsSoliderNeedToEatNextTurn(i_PlayerChoise.GetToSquare());               
            }
            if (m_otherPlayer.GetSoldiers().Capacity==0)
            {
                setGameStatus(m_currentPlayer);
            }

        }
        private void checkAndSetKingSolider(Soldier currentSoldier)
        {
            if(currentSoldier.GetRepresentChar()=='X')
            {
                if(currentSoldier.GetPlaceOnBoard().GetRow()=='a')
                {
                    currentSoldier.SetRepresentChar('K');
                    currentSoldier.SetTypeOfSoldeir(eSoldierType.King);
                }
            }
            else if(currentSoldier.GetRepresentChar() == 'O')
            {
                if (currentSoldier.GetPlaceOnBoard().GetRow() == (char)('a'+m_sizeOfBoard-1))
                {
                    currentSoldier.SetRepresentChar('U');
                    currentSoldier.SetTypeOfSoldeir(eSoldierType.King);
                }
            }
        }
        private void setParamatersIfIsSoliderNeedToEatNextTurn(Square i_Square)
        {
            m_isSoliderNeedToEatNextTurn = false;
            List<SquareMove> validMoves = new List<SquareMove>();
            List<SquareMove> mustToDoMoves = new List<SquareMove>();
            foreach (Soldier currentSolider in m_currentPlayer.GetSoldiers())
            {
                if (currentSolider.GetPlaceOnBoard().Equals(i_Square))
                {
                    validMoves = getValidMoveOfSolider(currentSolider);
                    initializeForMustMoves(validMoves, ref mustToDoMoves);
                    if(mustToDoMoves.Capacity>0)
                    {
                        m_isSoliderNeedToEatNextTurn=true;
                        m_soliderThatNeedToEatNextTurn = currentSolider;
                    }
                    break;
                }
            }
            
        }
        //Duplicate Code
        private void removeOtherPlayerSoliderFromBoard(SquareMove i_PlayerChoise)
        {

            char rowOfOtherPlayerToRemove=i_PlayerChoise.GetFromSquare().GetRow();
            char colOfOtherPlayerToRemove = i_PlayerChoise.GetFromSquare().GetCol();
            if ((i_PlayerChoise.GetToSquare().GetRow() - (i_PlayerChoise.GetFromSquare().GetRow()))> 0)
            {
                rowOfOtherPlayerToRemove += (char)1;
            }
            else
            {
                rowOfOtherPlayerToRemove -= (char)1;
            }
            if ((i_PlayerChoise.GetToSquare().GetCol() - (i_PlayerChoise.GetFromSquare().GetCol())) > 0)
            {
                colOfOtherPlayerToRemove += (char)1;
            }
            else
            {
                colOfOtherPlayerToRemove -= (char)1;
            }
            m_otherPlayer.RemoveSolider(new Square(rowOfOtherPlayerToRemove, colOfOtherPlayerToRemove));
        }
        private void setGameStatus(Player i_WineerPlayer=null)
        {
            if(i_WineerPlayer==null)
            {
                m_gameStatus = eGameStatus.Tie;
            }
            else
            {
                if (i_WineerPlayer.GetNumberOfPlayer() == eNumberOfPlayer.First)
                {
                    m_gameStatus = eGameStatus.FirstPlayerWon;

                }
                else
                {
                    m_gameStatus = eGameStatus.SecondPlayerWon;
                }
            }
        }
        private void calculateAndSetPoints(Player i_Winner,Player i_Loser)
        {
            int resultOfPoints= i_Winner.calculatePointsOfSoliders() - i_Loser.calculatePointsOfSoliders();
            i_Winner.setScore(i_Winner.getScore() + resultOfPoints);
        }
        private Player getPlayer(eNumberOfPlayer i_NumberOfPlayer)
        {
            Player playerToReturn;
            if(m_currentPlayer.GetNumberOfPlayer()==i_NumberOfPlayer)
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
            firstPlayer.GetSoldiers().Clear();
            secondPlayer.GetSoldiers().Clear();
            firstPlayer.generateSoliders(eNumberOfPlayer.First, m_sizeOfBoard);
            secondPlayer.generateSoliders(eNumberOfPlayer.Second, m_sizeOfBoard);
            m_currentPlayer = firstPlayer;
            m_otherPlayer = secondPlayer;
            m_gameStatus = eGameStatus.ContinueGame;
            m_isSoliderNeedToEatNextTurn = false;
            m_soliderThatNeedToEatNextTurn = null;
        }
    }
}
