using System;

namespace Program
{
    public class SquareMove : IEquatable<SquareMove>
    {
        private Square m_fromSquare;
        private Square m_toSquare;
        private bool m_mustDoMove;

        public SquareMove()
        {

        }
        public SquareMove(Square i_FromSquare,Square i_ToSquare,bool i_MustDoMove=false)
        {
            SetFromSquare(i_FromSquare);
            SetToSquare(i_ToSquare);
            SetMustDoMove(i_MustDoMove);
        }

        public static bool Parse(string moveFromClientS,out SquareMove o_SquareMove,eSizeBoard i_SizeOfBoard)
        {
            o_SquareMove = new SquareMove();
            bool isValidInput = true;
            char[] arrayofChars = moveFromClientS.ToCharArray();
            if (moveFromClientS.Length!=5 || arrayofChars[2] != '>')
            {
                isValidInput = false;
            }
            else if (!validRange(i_SizeOfBoard,'A', arrayofChars[0], arrayofChars[3]))
            {
                isValidInput = false;
            }
            else if (!validRange(i_SizeOfBoard,'a', arrayofChars[1], arrayofChars[4]))
            {
                isValidInput = false;
            }

            if(isValidInput==true)
            {
                o_SquareMove.SetFromSquare(new Square(arrayofChars[1], arrayofChars[0]));
                o_SquareMove.SetToSquare(new Square(arrayofChars[4], arrayofChars[3]));
            }
            return isValidInput;
        }

        private static bool validRange(eSizeBoard i_SizeOfBoard,char startRange,params char[] arrayToCheck)
        {
            bool validRows = true;
            foreach(char currentChar in arrayToCheck)
            {
                if(currentChar < startRange || currentChar > (char)(startRange + i_SizeOfBoard-1))
                {
                    validRows = false;
                }
            }
            return validRows;
        }

        //Setters
        public void SetToSquare(Square i_ToSquare)
        {
            m_toSquare = i_ToSquare;
        }
        public void SetFromSquare(Square i_FromSquare)
        {
            m_fromSquare = i_FromSquare;
        }
        private void SetMustDoMove(bool i_MustDoMove)
        {
            m_mustDoMove = i_MustDoMove;
        }
        //Getters
        public Square GetToSquare()
        {
            return m_toSquare;
        }
        public Square GetFromSquare()
        {
            return m_fromSquare;
        }
        public bool GetMustDoMove()
        {
            return m_mustDoMove;
        }
        public override string ToString()
        {
            return string.Format("{0}{1}{2}", m_fromSquare.ToString(), '>' , m_toSquare.ToString());
        }

        public bool Equals(SquareMove other)
        {
            bool isEqual=true;
            if(!this.m_fromSquare.Equals(other.m_fromSquare))
            {
                isEqual = false;
            }
            else if(!this.m_toSquare.Equals(other.m_toSquare))
            {
                isEqual = false;
            }
            return isEqual;
        }
    }


}
