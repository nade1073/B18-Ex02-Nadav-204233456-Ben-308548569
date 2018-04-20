
namespace Program
{

    class MovementOptions
    {
        public MovementOptions(eSizeBoard i_SizeOfBoard)
        {
            r_EndCol = (char)(k_StartCol + i_SizeOfBoard - 1);
            r_EndRow = (char)(k_StartRow + i_SizeOfBoard - 1);
        }

        public const char k_StartRow = 'a';
        public const char k_StartCol = 'A';

        private readonly char r_EndRow;
        private readonly char r_EndCol;


        private const int k_MoveUp = -1;
        private const int k_MoveDown = 1;
        private const int k_MoveRight = 1;
        private const int k_MoveLeft = -1;

        public char EndRow
        {
            get
            {
                return r_EndRow;
            }
        }
        public char EndCol
        {
            get
            {
                return r_EndCol;
            }
        }


        public int MoveUp
        {
            get
            {
                return k_MoveUp;
            }
        }
  
        public int MoveDown
        {
            get
            {
                return k_MoveDown;
            }
        }
        public int MoveRight
        {
            get
            {
                return k_MoveRight;
            }
        }
        public int MoveLeft
        {
            get
            {
                return k_MoveLeft;
            }
        }
    }
}
