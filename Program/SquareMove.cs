namespace Program
{
    using System;

    public class SquareMove : IEquatable<SquareMove>
    {
        private Square m_FromSquare;
        private Square m_ToSquare;
        private bool m_MustDoMove;

        public SquareMove(Square i_FromSquare = null, Square i_ToSquare = null, bool i_MustDoMove = false)
        {
            FromSquare = i_FromSquare;
            ToSquare = i_ToSquare;
            MustDoMove = i_MustDoMove;
        }

        public bool MustDoMove
        {
            get
            {
                return m_MustDoMove;
            }

            set
            {
                m_MustDoMove = value;
            }
        } 

        public Square ToSquare
        {
            get
            {
                return m_ToSquare;
            }

            set
            {
                m_ToSquare = value;
            }
        }

        public Square FromSquare
        {
            get
            {
                return m_FromSquare;
            }

            set
            {
                m_FromSquare = value;
            }
        }

        public static bool Parse(string i_MoveFromClientS, out SquareMove o_SquareMove, eSizeBoard i_SizeOfBoard)
        {
            o_SquareMove = new SquareMove();
            bool isValidInput = true;
            char[] arrayofChars = i_MoveFromClientS.ToCharArray();
            if (i_MoveFromClientS.Length != 5 || arrayofChars[2] != '>')
            {
                isValidInput = false;
            }
            else if (!validRange(i_SizeOfBoard, MovementOptions.k_StartCol, arrayofChars[0], arrayofChars[3]))
            {
                isValidInput = false;
            }
            else if (!validRange(i_SizeOfBoard, MovementOptions.k_StartRow, arrayofChars[1], arrayofChars[4]))
            {
                isValidInput = false;
            }

            if (isValidInput)
            {
                o_SquareMove.FromSquare = new Square(arrayofChars[1], arrayofChars[0]);
                o_SquareMove.ToSquare = new Square(arrayofChars[4], arrayofChars[3]);
            }

            return isValidInput;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}", FromSquare.ToString(), '>', ToSquare.ToString());
        }

        public bool Equals(SquareMove i_Other)
        {
            bool isEqual = true;
            if (!this.FromSquare.Equals(i_Other.FromSquare))
            {
                isEqual = false;
            }
            else if (!this.ToSquare.Equals(i_Other.ToSquare))
            {
                isEqual = false;
            }

            return isEqual;
        }

        private static bool validRange(eSizeBoard i_SizeOfBoard, char startRange, params char[] arrayToCheck)
        {
            bool validRows = true;
            foreach (char currentChar in arrayToCheck)
            {
                if (currentChar < startRange || currentChar > (char)(startRange + i_SizeOfBoard - 1))
                {
                    validRows = false;
                }
            }

            return validRows;
        }
    }
}
