using System;

namespace Program
{
    public class Soldier : IComparable<Soldier>
    {
        private char m_CharRepresent;
        private eSoldierType m_TypeOfSoldier;
        private Square m_PlaceOnBoard;

        //Defines 
        public const char k_FirstPlayerKing = 'U';
        public const char k_SecondPlayerKing = 'K';
        public const char k_FirstPlayerRegular = 'O';
        public const char k_SecondPlayerRegular = 'X';
        //Getters And Setters
        public char CharRepresent
        {
            set
            {
                m_CharRepresent = value;
            }
            get
            {
                return m_CharRepresent;
            }
        }
        public eSoldierType TypeOfSoldier
        {
            set
            {
                m_TypeOfSoldier = value;
            }
            get
            {
                return m_TypeOfSoldier;
            }
        }
        public Square PlaceOnBoard
        {
            set
            {
                m_PlaceOnBoard = value;
            }
            get
            {
                return m_PlaceOnBoard;
            }
        }
        //Constructor
        public Soldier(char i_CharRepresent,Square i_PlaceOnBoard ,eSoldierType i_TypeOfSolider = eSoldierType.Regular)
        {
            TypeOfSoldier = i_TypeOfSolider;
            PlaceOnBoard=i_PlaceOnBoard;
            CharRepresent = i_CharRepresent;
        }


        //Why compare to and not equal
        public int CompareTo(Soldier i_Other)
        {
            return this.m_PlaceOnBoard.Col.CompareTo(i_Other.m_PlaceOnBoard.Col);
        }
    }
 
}
