using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class BoardSquareScore
    {
    
        private readonly int[,] m_ArrayOfScore;
        public BoardSquareScore(eSizeBoard i_SizeOfBoard)
        {
            m_ArrayOfScore=generateArray(i_SizeOfBoard);
        }
        private int[,] generateArray(eSizeBoard i_SizeOfBoard)
        {
            int[,] returnedArr=null;   
            switch (i_SizeOfBoard)
            {
                case eSizeBoard.Six:
                    {
                        int[,] array = { {0,3,0,3,0,3},
                                         {3,0,2,0,2,0},
                                         {0,2,0,1,0,3},
                                         {3,0,1,0,2,0},
                                         {0,2,0,2,0,3},
                                         {3,0,3,0,3,0}   
                        };
                        returnedArr = array;
                        break;
                    }
                case eSizeBoard.Eight:
                    {
                        int[,] array = { {0,4,0,4,0,4,0,4},
                                         {4,0,3,0,3,0,3,0},
                                         {0,3,0,2,0,2,0,4},
                                         {4,0,2,0,1,0,3,0},
                                         {0,3,0,1,0,2,0,4},
                                         {4,0,2,0,2,0,3,0},
                                         {0,3,0,3,0,3,0,4},
                                         {4,0,4,0,4,0,4,0}
                        };
                        returnedArr = array;
                        break;
                    }
                case eSizeBoard.Ten:
                    {
                        int[,] array = { {0,5,0,5,0,5,0,5,0,5},
                                         {5,0,4,0,4,0,4,0,4,0},
                                         {0,4,0,3,0,3,0,3,0,5},
                                         {5,0,3,0,2,0,2,0,4,0},
                                         {0,4,0,2,0,1,0,3,0,5},
                                         {5,0,3,0,1,0,2,0,4,0},
                                         {0,4,0,2,0,2,0,3,0,5},
                                         {5,0,3,0,3,0,3,0,4,0},
                                         {0,4,0,4,0,4,0,4,0,5},
                                         {5,0,5,0,5,0,5,0,5,0}
                                       
                        };
                        returnedArr = array;
                        break;

                    }

            }
            return returnedArr;

        }

        public int[,] ArrayOfScores
        {
            get
            {
                return this.m_ArrayOfScore;
            }
        }
        
    }
}
