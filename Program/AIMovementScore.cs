namespace Program
{
    class AIMovementScore
    {
        private SquareMove m_FromToSquare;
        private int m_Score=0;

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
        public SquareMove SquareMove
        {
            get
            {
                return m_FromToSquare;
            }
        }

    }
}
