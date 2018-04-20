namespace Program
{   
    public class Square 
    {
        private char m_Row;
        private char m_Col;

        public Square(char i_Row, char i_Col)
        {
            Row = i_Row;
            Col = i_Col;
        }

        public char Row 
        {
            get
            {
                return m_Row;
            }

            set
            {
                m_Row = value;
            }
        }

        public char Col 
        {
            get
            {
                return m_Col;
            }

            set
            {
                m_Col = value;
            }
        }
        
        public override string ToString()
        {
            return string.Format("{0}{1}", m_Col, m_Row);
        }

        public override bool Equals(object i_Obj)
        {
            bool isEquals = true;
            var other = i_Obj as Square;
            if (other == null)
            {
                isEquals = false;
            }

            if (Row != other.Row || Col != other.Col)
            {
                isEquals = false;
            }

            return isEquals;
        }

        public override int GetHashCode()
        {
            return this.Row.GetHashCode();
        }
    }
}
