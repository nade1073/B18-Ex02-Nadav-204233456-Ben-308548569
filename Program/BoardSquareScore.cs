namespace Program
{
    public class BoardSquareScore
    {
        private readonly int[,] m_ArrayOfScore;

        public BoardSquareScore(eSizeBoard i_SizeOfBoard)
        {
            m_ArrayOfScore = generateArray(i_SizeOfBoard);
        }

        public int[,] ArrayOfScores
        {
            get
            {
                return m_ArrayOfScore;
            }
        }

        private int[,] generateArray(eSizeBoard i_SizeOfBoard)
        {
            int sizeOfBoardInteger = (int)i_SizeOfBoard;
            int[,] returnedArr = new int[sizeOfBoardInteger, sizeOfBoardInteger];
            int maxScore = sizeOfBoardInteger / 2;

            for (int i = 0; i < sizeOfBoardInteger / 2; i++)
            {
                for (int j = i; j < sizeOfBoardInteger; j += 2)
                {
                    returnedArr[i, j] = 0;
                    returnedArr[i, j + 1] = maxScore;
                    returnedArr[j, i] = 0;
                    returnedArr[j + 1, i] = maxScore;
                    returnedArr[sizeOfBoardInteger - 1, j] = maxScore;
                    returnedArr[sizeOfBoardInteger - 1, j + 1] = 0;
                    returnedArr[j, sizeOfBoardInteger - 1] = maxScore;
                    returnedArr[j + 1, sizeOfBoardInteger - 1] = 0;
                }

                maxScore--;
                sizeOfBoardInteger--;
            }

            return returnedArr;
        }   
    }
}
