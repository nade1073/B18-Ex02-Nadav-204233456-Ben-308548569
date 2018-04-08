
namespace Program
{
   
    public class Square 
    {
        private char row;
        private char col;

        public Square(char i_Row,char i_Col)
        {
            SetRow(i_Row);
            SetCol(i_Col);
        }
        public void SetRow(char i_Row)
        {
            this.row = i_Row;
        }
        public void SetCol(char i_Col)
        {
            this.col = i_Col;
        }
        public char GetCol()
        {
            return col;
        }
        public char GetRow()
        {
            return row;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}",row,col);
        }
        public override bool Equals(object obj)
        {
            var other = obj as Square;
            if (other == null)
                return false;
            if (row != other.row || col != other.col)
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            return this.row.GetHashCode();
        }
    }

}
