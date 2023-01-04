namespace XonixModelLibrary
{
    public class Cell
    {
        private readonly Area area;
        private CellState state;

        public Cell(Area area)
        {
            this.area = area;
        }

        public int Row { get; set; }
        public int Column { get; set; }
        
        public CellState State 
        {
            get { return state; }
            set
            {
                state = value;
                area[Row, Column] = (int)value;
            }
        }
    }
}
