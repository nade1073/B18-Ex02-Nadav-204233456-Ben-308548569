namespace Program
{
    public class Program
    {
        public static void Main()
        {
            Player firstPlayer, secondPlayer;
            eSizeBoard sizeOfBoard = UIUtilities.InitializePlayers(out firstPlayer, out secondPlayer);
            CheckerBoard board = new CheckerBoard(firstPlayer, secondPlayer, sizeOfBoard);
            board.startGame();      
        }
    }
}
