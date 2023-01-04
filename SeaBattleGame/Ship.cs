using System.Drawing;

namespace SeaBattleGame
{
    public class Ship
    {
        public int Desks { get; set; }
        public int Hits { get; set; }
        public Point[] Coordinates { get; set; }
        public Orientation Orientation { get; set; }
    }

    public enum Orientation
    {
        Unknown,
        Horizontal,
        Vertical,
    }
}
