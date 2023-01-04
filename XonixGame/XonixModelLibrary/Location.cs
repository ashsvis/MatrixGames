namespace XonixModelLibrary
{
    public struct Location
    {
        public Location(int dx, int dy)
        {
            X = dx;
            Y = dy;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public void Offset(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }
    }
}
