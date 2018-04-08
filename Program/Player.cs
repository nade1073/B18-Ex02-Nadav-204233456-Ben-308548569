using System;
using System.Collections.Generic;

namespace Program
{
    public class Player
    {
        private int m_score;
        private String m_playerName;
        private List<Soldier> m_soldiers;
        private eTypeOfPlayer m_typeOfPlayer;
        private eNumberOfPlayer m_numberOfPlayer;

        //Constructor
        public Player(String i_PlayerName,eTypeOfPlayer i_TypeOfPlayer,eNumberOfPlayer i_NumberOfPlayer,eSizeBoard i_BoardSize)
        {
     
            m_score = 0;
            m_playerName = i_PlayerName;
            m_typeOfPlayer = i_TypeOfPlayer;
            m_numberOfPlayer = i_NumberOfPlayer;
            m_soldiers = new List<Soldier>();
            generateSoliders(i_NumberOfPlayer, i_BoardSize);

        }


        //Getters
        public String GetPlayerName()
        {
            return m_playerName;
        }
        public List<Soldier> GetSoldiers()
        {
            return m_soldiers;
        }
        public eNumberOfPlayer GetNumberOfPlayer()
        {
            return m_numberOfPlayer;
        }
        public int getScore()
        {
            return m_score;
        }

        //Setters
        public void setScore(int i_Score)
        {
            m_score = i_Score;
        }


        public static bool isPlayerNameValid(String i_PlayerName)
        {
            
            bool isProperName = true;
            if(i_PlayerName.Length >20)
            {
                isProperName = false;
            }
            if(i_PlayerName.Contains(" "))
            {
                isProperName = false;
            }
            return isProperName;
        }
        public void generateSoliders(eNumberOfPlayer i_NumberOfPlayer,eSizeBoard i_BoardSize)
        {
            int numberOfRows = ((int)i_BoardSize/2)-1;
            int numberOfPlayersInRow = ((int)i_BoardSize / 2);
            char startRow;
            char representSoldier;

            switch(i_NumberOfPlayer)
            {
                case eNumberOfPlayer.First:
                    {
                        startRow = 'a';
                        representSoldier = 'O';
                        generateSolidersForPlayer(numberOfRows, numberOfPlayersInRow, startRow, representSoldier);
                        break;
                    }
                case eNumberOfPlayer.Second:
                    {
                        representSoldier = 'X';
                        startRow =(char)('a' +((int)(i_BoardSize)/2)+1);
                        generateSolidersForPlayer(numberOfRows, numberOfPlayersInRow, startRow, representSoldier);
                        break;
                    }
            }
        }
        private void generateSolidersForPlayer(int i_NumberOfRows,int i_NumberOfPlayersInRow,char i_startRow,char i_RepresentSoldier)
        {
            char startCol;
            for (int i=0;i< i_NumberOfRows; i++)
            {
                if ((int)i_startRow%2==1)
                {
                    startCol = 'B';
                }
                else
                {
                    startCol = 'A';
                }
                for (int j=0;j< i_NumberOfPlayersInRow; j++)
                {
                    m_soldiers.Add(new Soldier(i_RepresentSoldier, new Square(i_startRow, startCol)));
                    startCol = (char)(startCol + 2);
                }
                i_startRow++;
            }
        }
        
        public List<Soldier> getSoldierFromRaw(char i_Raw)
        {
            List<Soldier> soldiersFromSameRaw = new List<Soldier>();
            foreach(Soldier tempSoldier in m_soldiers)
            {
                if(tempSoldier.GetPlaceOnBoard().GetRow()==i_Raw)
                {
                    soldiersFromSameRaw.Add(tempSoldier);
                }
            }
            return soldiersFromSameRaw;
        }

        public void RemoveSolider(Square i_SoliderToRemove)
        {
            foreach(Soldier currentSolider in m_soldiers)
            {
                if(currentSolider.GetPlaceOnBoard().Equals(i_SoliderToRemove))
                {
                    m_soldiers.Remove(currentSolider);
                    break;
                }
            }
        }

        public int calculatePointsOfSoliders()
        {
            int result = 0;
            foreach(Soldier currentSolider in m_soldiers)
            {
                if(currentSolider.GetTypeOfSoldier()==eSoldierType.King)
                {
                    result += 4;
                }
                result += 1;
            }
            return result;
        }
      
    }
}
