using System;

namespace Program
{
    public class Soldier : IComparable<Soldier>
    {
        private char m_CharRepresent;
        private eSoldierType m_typeOfSoldier;
        private Square m_placeOnBoard;
        //Constructor

        public Soldier(char i_CharRepresent,Square i_PlaceOnBoard ,eSoldierType i_TypeOfSolider = eSoldierType.Regular)
        {
            SetTypeOfSoldeir(i_TypeOfSolider);
            SetPlaceOnBoard(i_PlaceOnBoard);
            SetRepresentChar(i_CharRepresent);
        }
        

        //Setters

        public void SetTypeOfSoldeir(eSoldierType i_TypeOfSolider)
        {
            this.m_typeOfSoldier = i_TypeOfSolider;
        }
        public void SetPlaceOnBoard(Square i_Place)
        {
            m_placeOnBoard = i_Place;
        }
        public void SetRepresentChar(char i_CharRepresent)
        {
            this.m_CharRepresent = i_CharRepresent;
        }
        //Getters
        public char GetRepresentChar()
        {
            return this.m_CharRepresent;
        }
        public Square GetPlaceOnBoard()
        {
            return this.m_placeOnBoard;
        }
        public eSoldierType GetTypeOfSoldier()
        {
            return this.m_typeOfSoldier;
        }

        public int CompareTo(Soldier other)
        {
            return this.m_placeOnBoard.GetCol().CompareTo(other.m_placeOnBoard.GetCol());
        }
    }
 
}
