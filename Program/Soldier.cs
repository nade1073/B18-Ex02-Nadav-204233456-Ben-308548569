namespace Program
{
    using System;

    public class Soldier : IComparable<Soldier>
    {
        public const char k_FirstPlayerKing = 'U';
        public const char k_SecondPlayerKing = 'K';
        public const char k_FirstPlayerRegular = 'O';
        public const char k_SecondPlayerRegular = 'X';
        public const char k_EmptySolider = ' ';
        private char m_CharRepresent;
        private eSoldierType m_TypeOfSoldier;
        private Square m_PlaceOnBoard;

        public Soldier(char i_CharRepresent, Square i_PlaceOnBoard, eSoldierType i_TypeOfSolider = eSoldierType.Regular)
        {
            TypeOfSoldier = i_TypeOfSolider;
            PlaceOnBoard = i_PlaceOnBoard;
            CharRepresent = i_CharRepresent;
        }

        public Soldier(Soldier i_CurrentSolider)
        {
            m_CharRepresent = i_CurrentSolider.m_CharRepresent;
            m_PlaceOnBoard = i_CurrentSolider.m_PlaceOnBoard;
            m_TypeOfSoldier = i_CurrentSolider.m_TypeOfSoldier;
        }

        public char CharRepresent
        {
            get
            {
                return m_CharRepresent;
            }

            set
            {
                m_CharRepresent = value;
            }
        }

        public eSoldierType TypeOfSoldier
        {
            get
            {
                return m_TypeOfSoldier;
            }

            set
            {
                m_TypeOfSoldier = value;
            }
        }

        public Square PlaceOnBoard
        {      
            get
            {
                return m_PlaceOnBoard;
            }

            set
            {
                m_PlaceOnBoard = value;
            }
        }

        public int CompareTo(Soldier i_Other)
        {
            return this.m_PlaceOnBoard.Col.CompareTo(i_Other.m_PlaceOnBoard.Col);
        }
    }
}
