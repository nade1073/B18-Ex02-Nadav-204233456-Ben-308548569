namespace Program
{
    public class AIMovementScore
    {
        private SquareMove m_FromToSquare;
        private double m_ScoreOfMove = 0;
        private int m_ScoreInBoard = 0;

        public AIMovementScore(SquareMove i_Square)
        {
            m_FromToSquare = i_Square;
        }

        public Square FromSquare
        {
            get
            {
                return m_FromToSquare.FromSquare;
            }
        }

        public Square ToSquare
        {
            get
            {
               return m_FromToSquare.ToSquare;
            }
        }

        public double ScoreOfMove
        {
            get
            {
                return m_ScoreOfMove;
            }

            set
            {
                m_ScoreOfMove = value;
            }
        }

        public SquareMove SquareMove
        {
            get
            {
                return m_FromToSquare;
            }
        }

        public int ScoreInBoard
        {
            get
            {
                return m_ScoreInBoard;
            }

            set
            {
                m_ScoreInBoard = value;
            }
        }
    }
}
