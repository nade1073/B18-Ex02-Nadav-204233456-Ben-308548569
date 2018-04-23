namespace Program
{
    using System;
    using System.Collections.Generic;

    public class Player
    {
        public const String k_computerName = "Computer";
        private int m_Score;
        private String m_PlayerName;
        private List<Soldier> m_Soldiers;
        private eTypeOfPlayer m_TypeOfPlayer;
        private eNumberOfPlayer m_NumberOfPlayer;

        public Player(String i_PlayerName, eTypeOfPlayer i_TypeOfPlayer, eNumberOfPlayer i_NumberOfPlayer, eSizeBoard i_BoardSize)
        {
            Score = 0;
            PlayerName = i_PlayerName;
            TypeOfPlayer = i_TypeOfPlayer;
            NumberOfPlayer = i_NumberOfPlayer;
            Soldiers = new List<Soldier>();
            generateSoliders(i_NumberOfPlayer, i_BoardSize);
        }

        public eNumberOfPlayer NumberOfPlayer
        {
            get
            {
                return m_NumberOfPlayer;
            }

            set
            {
                m_NumberOfPlayer = value;
            }
        }

        public eTypeOfPlayer TypeOfPlayer
        {
            get
            {
                return m_TypeOfPlayer;
            }

            set
            {
                m_TypeOfPlayer = value;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value;
            }
        }

        public string PlayerName
        {
            get
            {
                return m_PlayerName;
            }

            set
            {
                m_PlayerName = value;
            }
        }

        public List<Soldier> Soldiers
        {
            get
            {
                return m_Soldiers;
            }

            set
            {
                m_Soldiers = value;
            }
        }

        public static bool isPlayerNameValid(String i_PlayerName)
        {
            bool isProperName = true;
            if (i_PlayerName.Length > 20)
            {
                isProperName = false;
            }

            if (i_PlayerName.Contains(" "))
            {
                isProperName = false;
            }

            return isProperName;
        }

        public int getNumberOfSpesificSoldierType(eSoldierType i_Type)
        {
            int count = 0;
            foreach (Soldier currentSoldier in m_Soldiers)
            {
                if (currentSoldier.TypeOfSoldier == i_Type)
                {
                    count++;
                }
            }

            return count;
        }

        public void generateSoliders(eNumberOfPlayer i_NumberOfPlayer, eSizeBoard i_BoardSize)
        {
            Soldiers.Clear();
            int numberOfRows = ((int)i_BoardSize / 2) - 1;
            int numberOfPlayersInRow = (int)i_BoardSize / 2;
            char startRowForNumberOfPlayer;
            char representSoldier;

            switch (i_NumberOfPlayer)
            {
                case eNumberOfPlayer.First:
                    {
                        startRowForNumberOfPlayer = MovementOptions.k_StartRow;
                        representSoldier = Soldier.k_FirstPlayerRegular;
                        generateSolidersForPlayer(numberOfRows, numberOfPlayersInRow, startRowForNumberOfPlayer, representSoldier);
                        break;
                    }

                case eNumberOfPlayer.Second:
                    {
                        representSoldier = Soldier.k_SecondPlayerRegular;
                        startRowForNumberOfPlayer = (char)(MovementOptions.k_StartRow + ((int)i_BoardSize / 2) + 1);
                        generateSolidersForPlayer(numberOfRows, numberOfPlayersInRow, startRowForNumberOfPlayer, representSoldier);
                        break;
                    }
            }
        }
      
        public List<Soldier> getSoldierFromRaw(char i_Raw)
        {
            List<Soldier> soldiersFromSameRaw = new List<Soldier>();
            foreach (Soldier tempSoldier in Soldiers)
            {
                if (tempSoldier.PlaceOnBoard.Row == i_Raw)
                {
                    soldiersFromSameRaw.Add(tempSoldier);
                }
            }

            return soldiersFromSameRaw;
        }

        public void RemoveSolider(Square i_SoliderToRemove)
        {
            foreach (Soldier currentSolider in Soldiers)
            {
                if (currentSolider.PlaceOnBoard.Equals(i_SoliderToRemove))
                {
                    Soldiers.Remove(currentSolider);
                    break;
                }
            }
        }

        public int calculatePointsOfSoliders()
        {
            int result = 0;
            foreach (Soldier currentSolider in Soldiers)
            {
                if (currentSolider.TypeOfSoldier == eSoldierType.King)
                {
                    result += 4;
                }
                else
                {
                    result += 1;
                }
            }

            return result;
        }

        private void generateSolidersForPlayer(int i_NumberOfRows, int i_NumberOfPlayersInRow, char i_startRow, char i_RepresentSoldier)
        {
            char startCol;
            for (int i = 0; i < i_NumberOfRows; i++)
            {
                if ((int)i_startRow % 2 == 1)
                {
                    startCol = (char)(MovementOptions.k_StartCol + 1);
                }
                else
                {
                    startCol = MovementOptions.k_StartCol;
                }

                for (int j = 0; j < i_NumberOfPlayersInRow; j++)
                {
                    Soldiers.Add(new Soldier(i_RepresentSoldier, new Square(i_startRow, startCol)));
                    startCol = (char)(startCol + 2);
                }

                i_startRow++;
            }
        }
    }
}
