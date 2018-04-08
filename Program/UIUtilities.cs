using System;
using System.Collections.Generic;
using System.Text;

namespace Program
{
    class UIUtilities
    {
        public void nothing()
            {
            }
       public static eGameEndChoice getChoiseToContinuteTheGameFromClient()
       {
            bool isInputValid = false;
            String input;
            eGameEndChoice choiseToReturn=eGameEndChoice.Continue;
            Console.WriteLine("Write 'continue' to continue the game or 'end' to quit the game");
            while (!isInputValid)
            {
                input = Console.ReadLine();
                if(input.Equals("continue"))
                {
                    isInputValid = true;
                    choiseToReturn = eGameEndChoice.Continue;
                }
                else if(input.Equals("end"))
                {
                    isInputValid = true;
                    choiseToReturn = eGameEndChoice.Exit;
                }
                if(!isInputValid)
                {
                    Console.WriteLine("Write 'continue' to continue the game or 'end' to quit the game");
                }
            }
            return choiseToReturn;

       }
       public static String printCurrentMove(String i_PlayerName,char i_RepresentChar,SquareMove i_SquareMove)
        {
          return String.Format("{0}'s move was ({1}) : {2}",i_PlayerName,i_RepresentChar,i_SquareMove.ToString());
        }
       public static SquareMove getValidSquareToMoveFromClient(Player i_Player,eSizeBoard i_SizeOfBoard,List<SquareMove> i_AvaiableVaildMoves,List<SquareMove> i_MustToDoMoves)
       {
            bool quitFromGame=false;
            bool isValidMove = false;
            String moveFromClientS;
            SquareMove moveFromClient=null;
            Console.WriteLine(i_Player.GetPlayerName() + "'s turn:");
            while (!isValidMove)
            {
                do
                {
                    moveFromClientS = Console.ReadLine();
                    if (moveFromClientS.Equals("Q"))
                    {
                        quitFromGame = true;
                        break;
                    }
                } while (!(SquareMove.Parse(moveFromClientS, out moveFromClient, i_SizeOfBoard)));

                if(quitFromGame)
                {
                    break;
                }
                if(i_MustToDoMoves.Capacity>0)
                {
                    isValidMove = i_MustToDoMoves.Contains(moveFromClient);
                }
                else
                {
                    isValidMove = i_AvaiableVaildMoves.Contains(moveFromClient);
                }
            }
            return moveFromClient;
       }
       public static eSizeBoard InitializePlayers(out Player o_FirstPlayer,out Player o_SecondPlayer)
        {
            Console.WriteLine("Wellcome to the checker game\nDesigned and developed by Nadav Shalev & Ben Magriso\n");
            Console.WriteLine("Enter Your name and press enter");
            String firstPlayerName = getValidName();
            eSizeBoard sizeOfBoard = getSizeBoardFromClient();
            o_FirstPlayer = new Player(firstPlayerName, eTypeOfPlayer.Human, eNumberOfPlayer.First,sizeOfBoard);
            eTypeOfPlayer choiseTypeOfPlayer = getTypeOfPlayerFromClient();
            String secondPlayerName="computer";
            if (choiseTypeOfPlayer==eTypeOfPlayer.Human)
            {
                Console.WriteLine("Enter the second name player and press enter");
                secondPlayerName = getValidName();
            }
            o_SecondPlayer = new Player(secondPlayerName, choiseTypeOfPlayer, eNumberOfPlayer.Second, sizeOfBoard);
            return sizeOfBoard;
           
        }
       private static eSizeBoard getSizeBoardFromClient()
        {
            int sizeOfBoard;
            Console.WriteLine("Enter Your size of board and press enter -(6,8,10)");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out sizeOfBoard))
                {
                    if(sizeOfBoard==6 || sizeOfBoard == 8 || sizeOfBoard == 10)
                    {

                        break;
                    }
                    
                }
                Console.WriteLine("Player try again! Enter a valid size (6, 8, 10)");
            }
            return (eSizeBoard)sizeOfBoard;
        }
       private static String getValidName()
        {
            String playerName = Console.ReadLine();
            while (!Player.isPlayerNameValid(playerName))
            {
                Console.WriteLine("Please try again! Enter a valid name (20 letters, no spaces)");
                playerName = Console.ReadLine();
            }
            return playerName;
        }
       private static eTypeOfPlayer getTypeOfPlayerFromClient()
        {
            eTypeOfPlayer typeOfChoise;
            Console.WriteLine("Enter 'computer' to player Against the computer\nenter 'human' to playe Against Another Player and press enter");
            String choiseOfClient = Console.ReadLine();
            while(!(choiseOfClient.Equals("computer") || choiseOfClient.Equals("human")))
            {
                Console.WriteLine("Please try again!! valid names- (computer,human)");
               choiseOfClient = Console.ReadLine();
            }
            if(choiseOfClient.Equals("computer"))
            {
                typeOfChoise = eTypeOfPlayer.Computer;
            }
            else
            {
                typeOfChoise = eTypeOfPlayer.Human;
            }
            return typeOfChoise;
        }
       public static void PrintBoard(Player i_FirstPlayer,Player i_SecondPlayer,int i_Size)
        {
               
            StringBuilder board = new StringBuilder();
            StringBuilder rawFormat = buildRawFormat(i_Size + 1);
            StringBuilder headLine = buildHeadLine(i_Size);
            StringBuilder equalsLine = builderEqualsLine(i_Size);
            board.AppendLine(headLine.ToString()).AppendLine(equalsLine.ToString());
            char startRow = 'a';
            for (int i=0;i< i_Size; i++)
            {
                String rawForBoard = generateRawForBoard(i_FirstPlayer, i_SecondPlayer, startRow,i_Size, rawFormat);
                board.AppendLine(rawForBoard).AppendLine(equalsLine.ToString());
                startRow++;

            }
            Console.WriteLine(board);

        }
       public static void printResultOnScreen(Player i_FirstPlayer, Player i_SecondPlayer)
       {
            string outPutMessage = String.Format("{0} has {1} points \n{2} has {3} poitns", i_FirstPlayer.GetPlayerName(), i_FirstPlayer.getScore(), i_SecondPlayer.GetPlayerName(), i_SecondPlayer.getScore());
            System.Console.WriteLine(outPutMessage);
       }
       private static StringBuilder buildRawFormat(int i_Size)
       {
            StringBuilder raw =new StringBuilder();
            for (int i = 0; i < i_Size; i++)
            {
                raw.Append(" {" + i + "} |");
            }
            return raw;
        }
       private static StringBuilder buildHeadLine(int i_Size)
       {
            char startLetter = 'A';
            StringBuilder raw = new StringBuilder();
            raw.Append("  ");
            for(int i=0;i<i_Size;i++)
            {
                raw.Append("    " +startLetter + " ");
                startLetter++;
            }
            return raw;
        }
       private static StringBuilder builderEqualsLine(int i_Size)
        {
            StringBuilder raw = new StringBuilder();
            raw.Append("   ");
            for (int i = 0; i < i_Size; i++)
            {
                raw.Append("======");
            }
            return raw;
        }
       private static String generateRawForBoard(Player i_FirstPlayer,Player i_SecondPlayer,char i_Raw,int i_Size,StringBuilder i_RawFormat)
       {
            List<String> paramsForRaw = new List<String>();
            List<Soldier> soldiersForEachRaw = new List<Soldier>();
            char indexCol = 'A';
            int indexForList=0;
            soldiersForEachRaw.AddRange(i_FirstPlayer.getSoldierFromRaw(i_Raw));
            soldiersForEachRaw.AddRange(i_SecondPlayer.getSoldierFromRaw(i_Raw));
            paramsForRaw.Add(i_Raw.ToString());
            soldiersForEachRaw.Sort();
            for (int j = 0; j < i_Size; j++)
            {
                if (indexForList < soldiersForEachRaw.Count)
                {
                    if (soldiersForEachRaw[indexForList].GetPlaceOnBoard().GetCol() == indexCol)
                    {
                        paramsForRaw.Add(' ' + soldiersForEachRaw[indexForList].GetRepresentChar().ToString() + ' ');
                        indexForList++;
                    }
                    else
                    {
                        paramsForRaw.Add("   ");
                    }
                }
                else
                {
                    paramsForRaw.Add("   ");
                }

                indexCol++;
            }
            return String.Format(i_RawFormat.ToString(),paramsForRaw.ToArray());
        }
    
    }
}
