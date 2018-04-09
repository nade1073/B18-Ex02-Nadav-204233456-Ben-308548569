namespace Program
{
    class Program
    {
        public static void Main()
        {

            Player firstPlayer,secondPlayer;
            //Logic???
            eSizeBoard sizeOfBoard=UIUtilities.InitializePlayers(out firstPlayer,out secondPlayer);
            CheckerBoard board = new CheckerBoard(firstPlayer, secondPlayer, sizeOfBoard);
            board.startGame();
        
        }
    }
}
